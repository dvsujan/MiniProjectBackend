const getReviewsFromServer = async (id) => {
  var res = await fetch(`http://localhost:5122/api/Review?bookId=${id}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + localStorage.getItem("token"),
    },
  });
  var data = await res.json();
  return data;
};

const getBookDataFromServer = async (id) => {
  var res = await fetch(`http://localhost:5122/api/Book/get?id=${id}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + localStorage.getItem("token"),
    },
  });
  var data = await res.json();
  return data;
};


const getReviewStringFromInt = (rating) => {
  var starstr = "";
  for (var i = 0; i < rating; i++) {
    starstr += "★";
  }
  for (var i = rating; i < 5; i++) {
    starstr += "☆";
  }
  return starstr;
};

window.addEventListener("load", async function () {
  if (!localStorage.getItem("token")) {
    window.location.href = "login.html";
  }
  var url = new URL(window.location.href);
  var id = url.searchParams.get("id");
  if (id == null) {
    window.location.href = "index.html";
  }
  var bookData = await getBookDataFromServer(id);
  var googleBook = await getBookDataFromGoogle(bookData.isbn);
  this.document.getElementById("description").innerHTML =
    googleBook.description;
  this.document.getElementById("book-cover").src = googleBook.thumbnail;
  this.document.getElementById(
    "author"
  ).innerHTML = `Author: ${googleBook.author}`;
  this.document.getElementById(
    "category"
  ).innerHTML = `Category: ${googleBook.category}`;
  this.document.getElementById(
    "pubDate"
  ).innerHTML = `Published: ${googleBook.published}`;
  this.document.getElementById(
    "pages"
  ).innerHTML = `PageCount: ${googleBook.pageCount}`;
  this.document.getElementById("isbn").innerHTML = `ISBN: ${bookData.isbn}`;
  this.document.getElementById(
    "language"
  ).innerHTML = `Language: ${googleBook.language}`;
  var reviews = await getReviewsFromServer(id);
  const reviewList = document.getElementById("reviews");
  for (var x = 0; x < reviews.length; x++) {
    const review = reviews[x];
    var card = document.createElement("div");
    card.classList.add("review-card");
    card.innerHTML = `
		 <span>
                            <h4>${review.userName}</h4>
                            <span class="stars">${getReviewStringFromInt(review.rating)}</span>
                        </span>
                        <p>
							${review.comment}
                        </p>`;
	reviewList.appendChild(card);
  }
  var myTabs = document.querySelectorAll("ul.nav-tabs > li");
  function myTabClicks(tabClickEvent) {
    for (var i = 0; i < myTabs.length; i++) {
      myTabs[i].classList.remove("active");
    }
    var clickedTab = tabClickEvent.currentTarget;
    clickedTab.classList.add("active");
    tabClickEvent.preventDefault();
    var myContentPanes = document.querySelectorAll(".tab-pane");
    for (i = 0; i < myContentPanes.length; i++) {
      myContentPanes[i].classList.remove("active");
    }
    var anchorReference = tabClickEvent.target;
    var activePaneId = anchorReference.getAttribute("href");
    var activePane = document.querySelector(activePaneId);
    activePane.classList.add("active");
  }
  for (i = 0; i < myTabs.length; i++) {
    myTabs[i].addEventListener("click", myTabClicks);
  }
});

const borrowBook = async () => {
	var bookId = new URL(window.location.href).searchParams.get("id");
	var userId = parseJwt(localStorage.getItem("token")).UserId;
	var date = new Date();
	var data = {
		bookId: bookId,
		userId: userId,
		borrowDate: date,
	};
	var res = await fetch("http://localhost:5122/api/Borrow/borrow", {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
			Authorization: "Bearer " + localStorage.getItem("token"),
		},
		body: JSON.stringify(data),
	});
	var response = await res.json();
	if (res.status == 200) {
		spwanSnackBar("Book borrowed successfully", "green");
	} else {
		spwanSnackBar(response.message);
	}
}

const submitReview = async () => {
	var bookId = new URL(window.location.href).searchParams.get("id");
	var userId = parseJwt(localStorage.getItem("token")).UserId;
	var comment = document.getElementById("review-comment").value;
	comment = comment.replace(/^\s+/, '');
	if(comment == ""){
		spwanSnackBar("Please enter a comment", "blue");
		return;
	}
	var rating = 0 ; 
	try{ 
		rating = document.querySelector('input[name="rating"]:checked').value;
		rating = parseInt(rating);
	}
	catch(err){
		spwanSnackBar("Please select a rating");
		return;
	}
	var data = {
		bookId: bookId,
		userId: userId,
		comment: comment,
		rating: rating,
	};
	var res = await fetch("http://localhost:5122/api/Review", {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
			Authorization: "Bearer " + localStorage.getItem("token"),
		},
		body: JSON.stringify(data),
	});
	var response = await res.json();
	if (res.status == 200) {
		spwanSnackBar("Review added successfully", "green");
	} else {
		spwanSnackBar(response.message);
	}
}

const reserveBook = async () => {
	var bookId = new URL(window.location.href).searchParams.get("id");
	var userId = parseJwt(localStorage.getItem("token")).UserId;
	var date = document.getElementById("date").value;
	var data = {
		bookId: bookId,
		userId: userId,
		reserveDate: date,
	};
	console.log(data); 
	if(date == ""){
		spwanSnackBar("Please select a date", "blue");
		return;
	}
	if (new Date(date) < new Date()) {
		spwanSnackBar("Please select a future date", "blue");
		return;
	}
	var res = await fetch("http://localhost:5122/api/Reservation/reserve", {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
			Authorization: "Bearer " + localStorage.getItem("token"),
		},
		body: JSON.stringify(data),
	});
	var response = await res.json();
	if (res.status == 200) {
		spwanSnackBar("Book reserved successfully", "green");
	} else {
		spwanSnackBar(response.message);
	}
}

