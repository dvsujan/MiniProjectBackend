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
  const res = await fetchWithTimeout(
    url,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    },
    5000
  );
  if (res === "timeout") {
    spwanSnackBar("Request timed out");
  }
  const data = await res.json();
  return data;
};

const returnFunc = async (bookId) => {
  const url = "http://localhost:5122/api/Borrow/return";
  const data = {
    userId: parseJwt(localStorage.getItem("token")).UserId,
    bookId: bookId,
  };
  const res = await fetchWithTimeout(
    url,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
      body: JSON.stringify(data),
    },
    5000
  );
  if (res === "timeout") {
    spwanSnackBar("Request timed out");
  }
  const response = await res.json();
  if (res.status == 200) {
    spwanSnackBar("Book returned successfully", "green");
    setTimeout(() => {
      location.reload();
    }, 1000);
  } else {
    spwanSnackBar(response.message);
  }
  hideLoadingScreen();
};
const PayFine = async (borrowId) => {
  window.location.href = `/PayGateway.html?borrowId=${borrowId}`;
};

const renewFunc = async (bookId) => {
  const url = `http://localhost:5122/api/Borrow/renew?userId=${
    parseJwt(localStorage.getItem("token")).UserId
  }&bookId=${bookId}`;
    const res = await fetchWithTimeout(
        url,
        {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + localStorage.getItem("token"),
        },
        },
        5000
    );
    if (res === "timeout") {
        spwanSnackBar("Request timed out");
    }
    const response = await res.json();
    if (res.status == 200) {
        spwanSnackBar("Book renewed successfully", "green");
        setTimeout(() => {
            location.reload();
        }, 1000);
    } else {
        spwanSnackBar(response.message);
    }
};

const ConstructBorrowCard = async (book) => {
  const bookData = await GetBookDataFromApi(book.bookId);
  return `
    <div class="borrowed-card">
            <h1>${bookData.title}</h1>
            <p>borrowDate: ${book.borrowDate.split("T")[0]}</p>
            <p>dueDate: ${book.dueDate.split("T")[0]}</p>
            <div class="borrowed-btn">
                <button onClick="renewFunc(${book.bookId})">Renew</button>
                <button onClick="returnFunc(${book.bookId})">Return</button>
            </div>
    `;
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
                <button onClick="PayFine(${book.borrowId})">Pay Fine</button>
            </div>
    </div>
    `;
};

const fetchBorrowedBooks = async () => {
  const url =
    "http://localhost:5122/api/Borrow/borrowed" +
    "?userId=" +
    parseJwt(localStorage.getItem("token")).UserId;
  const cardContainer = document.getElementById("card-container");
  const dueCardContainer = document.getElementById("due-card-container");
  const res = await fetchWithTimeout(
    url,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    },
    5000
  );
  if (res === "timeout") {
    spwanSnackBar("Request timed out");
  }
  const data = await res.json();
  if (data.length == 0) {
    dueCardContainer.innerHTML = "<h1>No books due</h1>";
    cardContainer.innerHTML = "<h1>No books borrowed</h1>";
    hideLoadingScreen();
    return;
  }
  data.forEach(async (book) => {
    if (new Date(book.dueDate) < new Date()) {
      const card = await ConstructDueCard(book);
      dueCardContainer.innerHTML += card;
    } else {
      const card = await ConstructBorrowCard(book);
      cardContainer.innerHTML += card;
    }
  });

  // if (dueCardContainer.innerHTML  == "") {
  //     dueCardContainer.innerHTML = "<h1>No books due</h1>";
  // }
  hideLoadingScreen();
};

const deleteReview = async (reviewId) => {
  const url = `http://localhost:5122/api/Review?reviewId=${reviewId}&userId=${parseInt(
    parseJwt(localStorage.getItem("token")).UserId
  )}`;
  const res = await fetchWithTimeout(
    url,
    {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    },
    5000
  );
  if (res === "timeout") {
    spwanSnackBar("Request timed out");
  }

  const response = await res.json();
  if (res.status == 200) {
    spwanSnackBar("Review deleted successfully", "green");
    setTimeout(() => {
      location.reload();
    }, 1000);
  } else {
    spwanSnackBar(response.message);
  }
};

const ConstructReviewCard = (review) => {
  return `
    <div class="review-card">
        <h1>${review.bookTitle}</h1>
        <p><strong>Rating:</strong> ${review.rating}</p>
        <p><strong>Review: </strong>${review.comment}</p>
        <button onClick="deleteReview(${review.reviewId})">delete</button>
    </div>
    `;
};

const fetchUseReviews = async () => {
  const url =
    "http://localhost:5122/api/Review/user" +
    "?userId=" +
    parseJwt(localStorage.getItem("token")).UserId;
  const res = await fetchWithTimeout(
    url,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("token"),
      },
    },
    5000
  );
  if (res === "timeout") {
    spwanSnackBar("Request timed out");
  }
  const data = await res.json();
  data.forEach((review) => {
    const card = ConstructReviewCard(review);
    document.getElementById("review-card-container").innerHTML += card;
  });
  hideLoadingScreen();
};

$(document).ready(function () {
  fetchBorrowedBooks();
  fetchUseReviews();
  openCity(event, "Borrowed");
});
