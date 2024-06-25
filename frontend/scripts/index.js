$(document).ready(function () {
  if (localStorage.getItem("token")) {
    var token = localStorage.getItem("token");
    var user = parseJwt(token);
    var now = new Date();
    var expiry = new Date(user.exp * 1000);
    if (now > expiry) {
      localStorage.removeItem("token");
      window.location.href = "login.html";
    }
    var url = new URL(window.location.href);
    var page = url.searchParams.get("page") || 1;
    console.log(page);
    document.getElementById("page").innerHTML = `${page}`;
    if (url.searchParams.get("search")) {
      var title = url.searchParams.get("search");
      loadSearchedBooks(title);
    } else {
      loadAllBooks();
    }
  }
});

const getBookFromGoogle = async (isbn) => {
  const url = `https://www.googleapis.com/books/v1/volumes?q=isbn:${isbn}`;
  var res = await fetchWithTimeout(
    url,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    },
    5000
  );
  if (res === "timeout") {
    return {
      thumbnail: "https://via.placeholder.com/150",
      description: "No description available",
      title: "Book Title",
    };
  }

  var data = await res.json();
  var thumbnail = data.items[0].volumeInfo.imageLinks.thumbnail;
  var description = data.items[0].volumeInfo.description;
  var title = data.items[0].volumeInfo.title;
  return {
    thumbnail: thumbnail,
    description: description,
    title: title,
  };
};

const loadSearchedBooks = async (title) => {
  const page = new URL(window.location.href).searchParams.get("page") || 1;
  const url = `http://localhost:5122/api/Book/search?title=${title}&page=${page}`;
  var res = await fetchWithTimeout(
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
    return;
  }
  var data = await res.json();
  console.log(data);
  var bookList = document.getElementById("all-books");
  bookList.innerHTML = "";
  for (var x = 0; x < data.length; x++) {
    const book = data[x];
    console.log(book);
    var googledata = await getBookFromGoogle(book.isbn);
    var card = document.createElement("div");
    card.classList.add("card");
    var cardImage = document.createElement("div");
    cardImage.classList.add("card-image");
    var img = document.createElement("img");
    img.src = `${googledata.thumbnail}`;
    img.alt = book.title;
    cardImage.appendChild(img);
    card.appendChild(cardImage);
    var cardContent = document.createElement("div");
    cardContent.classList.add("card-content");
    var cardHeader = document.createElement("div");
    cardHeader.classList.add("card-header");
    var title = document.createElement("h2");
    title.classList.add("card-title");
    title.innerHTML = book.title;
    var cardRating = document.createElement("div");
    cardRating.classList.add("card-rating");
    var stars = document.createElement("span");
    stars.classList.add("stars");
    var starstr = "";
    for (var i = 0; i < book.rating; i++) {
      starstr += "★";
    }
    for (var i = book.rating; i < 5; i++) {
      starstr += "☆";
    }
    stars.innerHTML = starstr;
    var ratingCount = document.createElement("span");
    ratingCount.classList.add("rating-count");
    ratingCount.innerHTML = `(${book.noOfRatings})`;
    cardRating.appendChild(stars);
    cardRating.appendChild(ratingCount);
    cardHeader.appendChild(title);
    cardHeader.appendChild(cardRating);
    cardContent.appendChild(cardHeader);
    var subtitle = document.createElement("p");
    subtitle.classList.add("card-subtitle");
    subtitle.innerHTML = googledata.description;
    cardContent.appendChild(subtitle);
    var readMore = document.createElement("button");
    readMore.innerHTML = "Read More";
    readMore.onclick = function () {
      window.location.href = `book.html?id=${book.bookId}`;
    };
    cardContent.appendChild(readMore);
    card.appendChild(cardContent);
    bookList.appendChild(card);
  }
};

const loadAllBooks = async () => {
  const page = new URL(window.location.href).searchParams.get("page") || 1;
  const url = `http://localhost:5122/api/Book/all?page=${page}`;

  var res = await fetchWithTimeout(
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
    return;
  }
  console.log(res);
  var data = await res.json();
  console.log(data);
  var bookList = document.getElementById("all-books");

  for (var x = 0; x < data.length; x++) {
    const book = data[x];
    console.log(book);
    var googledata = await getBookFromGoogle(book.isbn);
    var card = document.createElement("div");
    card.classList.add("card");
    var cardImage = document.createElement("div");
    cardImage.classList.add("card-image");
    var img = document.createElement("img");
    img.src = `${googledata.thumbnail}`;
    img.alt = book.title;
    cardImage.appendChild(img);
    card.appendChild(cardImage);
    var cardContent = document.createElement("div");
    cardContent.classList.add("card-content");
    var cardHeader = document.createElement("div");
    cardHeader.classList.add("card-header");
    var title = document.createElement("h2");
    title.classList.add("card-title");
    title.innerHTML = book.title;
    var cardRating = document.createElement("div");
    cardRating.classList.add("card-rating");
    var stars = document.createElement("span");
    stars.classList.add("stars");
    var starstr = "";
    for (var i = 0; i < book.rating; i++) {
      starstr += "★";
    }
    for (var i = book.rating; i < 5; i++) {
      starstr += "☆";
    }
    stars.innerHTML = starstr;
    var ratingCount = document.createElement("span");
    ratingCount.classList.add("rating-count");
    ratingCount.innerHTML = `(${book.noOfRatings})`;
    cardRating.appendChild(stars);
    cardRating.appendChild(ratingCount);
    cardHeader.appendChild(title);
    cardHeader.appendChild(cardRating);
    cardContent.appendChild(cardHeader);
    var subtitle = document.createElement("p");
    subtitle.classList.add("card-subtitle");
    subtitle.innerHTML = googledata.description;
    cardContent.appendChild(subtitle);
    var readMore = document.createElement("button");
    readMore.innerHTML = "Read More";
    readMore.onclick = function () {
      window.location.href = `book.html?id=${book.bookId}`;
    };
    cardContent.appendChild(readMore);
    card.appendChild(cardContent);
    bookList.appendChild(card);
  }
};

const prevPage = () => {
  var url = new URL(window.location.href);
  var page = url.searchParams.get("page") || 1;
  page = parseInt(page);
  if (page > 1) {
    page--;
  }
  if (url.searchParams.get("search")) {
    var title = url.searchParams.get("search");
    window.location.href = `index.html?search=${title}&page=${page}`;
  }
  else{ 
    window.location.href = `index.html?page=${page}`;
  }
};
const nextPage = () => {
  var url = new URL(window.location.href);
  var page = url.searchParams.get("page") || 1;
  page = parseInt(page);
  page++;
  if (url.searchParams.get("search")) {
    var title = url.searchParams.get("search");
    window.location.href = `index.html?search=${title}&page=${page}`;
  }
  else{ 
    window.location.href = `index.html?page=${page}`;
  }
};

const searchBooks = () => {
  var title = document.getElementById("search-bar").value;
  if (title === "") {
    spwanSnackBar("Book title cannot be empty");
    return;
  }
  window.location.href = `index.html?search=${title}`;
}
