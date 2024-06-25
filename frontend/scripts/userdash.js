const hideAllTabs = (tabcontent) => {
  for (let i = 0; i < tabcontent.length; i++) {
    tabcontent[i].style.display = "none";
  }
};

const removeActiveClass = (tablinks) => {
  for (let i = 0; i < tablinks.length; i++) {
    tablinks[i].className = tablinks[i].className.replace(" active", "");
  }
};

const openCity = (evt, cityName) => {
  hideAllTabs(document.getElementsByClassName("tabcontent"));
  removeActiveClass(document.getElementsByClassName("tablinks"));

  document.getElementById(cityName).style.display = "block";
  evt.currentTarget.className += " active";
};

const GetBookDataFromApi = async (id) => {
    const url = `http://localhost:5122/api/Book/get?id=${id}`;
    const res = await fetch(url, {
        method: "GET",
        headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
        },
    });
    const data = await res.json();
    return data;

}

const ConstructBorrowCard = async (book) => {
    const bookData = await GetBookDataFromApi(book.bookId); 
    return `
    <div class="borrowed-card">
            <h1>${bookData.title}</h1>
            <p>borrowDate: ${book.borrowDate.split("T")[0]}</p>
            <p>dueDate: ${book.dueDate.split("T")[0]}</p>
            <div class="borrowed-btn">
                <button>Return</button>
            </div>
    `
}; 
const ConstructDueCard = async (book) => {
    const bookData = await GetBookDataFromApi(book.bookId);
    return `
    <div class="borrowed-card">

            <h1>${bookData.title}</h1>
            <p>BorrowDate: ${book.borrowDate.split("T")[0]}</p>
            <p>DueDate: ${book.dueDate.split("T")[0]}</p>
            <p>Fine: ${book.fine}</p>
            <div class="due-borrowed-btn">
                <button>Pay Fine</button>
            </div>
    </div>
    `
};



const fetchBorrowedBooks = async ()=>{ 
    const url = "http://localhost:5122/api/Borrow/borrowed"+ "?userId=" + parseJwt(localStorage.getItem("token")).UserId;
    const cardContainer = document.getElementById("card-container");
    const dueCardContainer = document.getElementById("due-card-container");
    const res = await fetch(url, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + localStorage.getItem("token"),
        },
    });
    const data = await res.json();
    data.forEach(async book => {
        if(new Date(book.dueDate) < new Date()){
            const card = await ConstructDueCard(book);
            dueCardContainer.innerHTML += card;
        }
        else{ 
            const card = await ConstructBorrowCard(book);
            cardContainer.innerHTML += card;
        }
    });
}

$(document).ready(function () { 
    fetchBorrowedBooks();
    openCity(event, "Borrowed");
}) ; 