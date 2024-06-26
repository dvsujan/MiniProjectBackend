const getReviewsFromServer = async (id) => {
  const res = await fetchWithTimeout(`http://localhost:5122/api/Review?bookId=${id}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  }, 5000);
  if (res === "timeout") {
    spwanSnackBar("Request timed out");
    return [];
  }
  return res.json();
};

const getBookDataFromServer = async (id) => {
  const res = await fetch(`http://localhost:5122/api/Book/get?id=${id}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
  });
  return res.json();
};

const getReviewStringFromInt = (rating) => {
  return "★".repeat(rating) + "☆".repeat(5 - rating);
};

const displayBookDetails = (bookData, googleBook) => {
  document.getElementById("description").innerHTML = googleBook.description;
  document.getElementById("book-cover").src = `https://covers.openlibrary.org/b/isbn/${bookData.isbn}-L.jpg`;
  document.getElementById("author").innerHTML = `Author: ${googleBook.author}`;
  document.getElementById("category").innerHTML = `Category: ${googleBook.category}`;
  document.getElementById("pubDate").innerHTML = `Published: ${googleBook.published}`;
  document.getElementById("pages").innerHTML = `PageCount: ${googleBook.pageCount}`;
  document.getElementById("isbn").innerHTML = `ISBN: ${bookData.isbn}`;
  document.getElementById("language").innerHTML = `Language: ${googleBook.language}`;
};

const displayReviews = (reviews) => {
  const reviewList = document.getElementById("reviews");
  reviews.forEach(review => {
    const card = document.createElement("div");
    card.classList.add("review-card");
    card.innerHTML = `
      <span>
        <h4>${review.userName}</h4>
        <span class="stars">${getReviewStringFromInt(review.rating)}</span>
      </span>
      <p>${review.comment}</p>`;
    reviewList.appendChild(card);
  });
};

const loadBookDetails = async () => {
  showLoadingScreen();
  if (!localStorage.getItem("token")) {
    window.location.href = "login.html";
    return;
  }
  const url = new URL(window.location.href);
  const id = url.searchParams.get("id");
  if (!id) {
    window.location.href = "index.html";
    return;
  }
  const bookData = await getBookDataFromServer(id);
  const googleBook = await getBookDataFromGoogle(bookData.isbn);
  displayBookDetails(bookData, googleBook);
  const reviews = await getReviewsFromServer(id);
  displayReviews(reviews);
  initTabs();
  hideLoadingScreen();
};

window.addEventListener("load", loadBookDetails);

const postRequest = async (url, data) => {
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${localStorage.getItem("token")}`,
    },
    body: JSON.stringify(data),
  });
  return {
	data:await res.json(),
	success: res.status === 200
 };
};

const borrowBook = async () => {
  const bookId = new URL(window.location.href).searchParams.get("id");
  const userId = parseJwt(localStorage.getItem("token")).UserId;
  const date = new Date();
  const data = { bookId, userId, borrowDate: date };
  const res = await postRequest("http://localhost:5122/api/Borrow/borrow", data);
  spwanSnackBar(res.success  ? "Book borrowed successfully" : res.data.message, res.success ? "green" : undefined);
};

const submitReview = async () => {
  const bookId = new URL(window.location.href).searchParams.get("id");
  const userId = parseJwt(localStorage.getItem("token")).UserId;
  const comment = document.getElementById("review-comment").value.trim();
  if (!comment) {
    spwanSnackBar("Please enter a comment", "blue");
    return;
  }
  let rating;
  try {
    rating = parseInt(document.querySelector('input[name="rating"]:checked').value);
  } catch {
    spwanSnackBar("Please select a rating");
    return;
  }
  const data = { bookId, userId, comment, rating };
  const res = await postRequest("http://localhost:5122/api/Review", data);
  spwanSnackBar(res.success ? "Review submitted successfully" : res.data.message, res.success ? "green" : undefined);
};

const reserveBook = async () => {
  const bookId = new URL(window.location.href).searchParams.get("id");
  const userId = parseJwt(localStorage.getItem("token")).UserId;
  const date = document.getElementById("date").value;
  if (!date) {
    spwanSnackBar("Please select a date", "blue");
    return;
  }
  if (new Date(date) < new Date()) {
    spwanSnackBar("Please select a future date", "blue");
    return;
  }
  const data = { bookId, userId, reserveDate: date };
  const res = await postRequest("http://localhost:5122/api/Reservation/reserve", data);
  spwanSnackBar(res.success ? "Book reserved successfully" : res.data.message, res.success ? "green" : undefined);
};

const initTabs = () => {
  const myTabs = document.querySelectorAll("ul.nav-tabs > li");
  myTabs.forEach(tab => {
    tab.addEventListener("click", function (tabClickEvent) {
      myTabs.forEach(tab => tab.classList.remove("active"));
      tabClickEvent.currentTarget.classList.add("active");
      tabClickEvent.preventDefault();
      const myContentPanes = document.querySelectorAll(".tab-pane");
      myContentPanes.forEach(pane => pane.classList.remove("active"));
      const activePaneId = tabClickEvent.target.getAttribute("href");
      document.querySelector(activePaneId).classList.add("active");
    });
  });
};

