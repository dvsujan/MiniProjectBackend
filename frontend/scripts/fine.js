var SelectedCard = -1;

function GetCardType(number) {
  var re = new RegExp("^4");
  if (number.match(re) != null) return "Visa";

  if (
    /^(5[1-5][0-9]{14}|2(22[1-9][0-9]{12}|2[3-9][0-9]{13}|[3-6][0-9]{14}|7[0-1][0-9]{13}|720[0-9]{12}))$/.test(
      number
    )
  )
    return "Mastercard";
  return "None";
}
const constructCard = async (card) => {
  var cardElement = document.createElement("span");
  cardElement.classList.add("cards");
  var img = document.createElement("img");
  if (GetCardType(card.cardNumber) == "Visa") {
    img.src = "https://img.icons8.com/color/48/000000/visa.png";
  } else if (GetCardType(card.cardNumber) == "Mastercard") {
    img.src = "https://img.icons8.com/color/48/000000/mastercard-logo.png";
  } else {
    img.src = "./assets/card.png";
  }
  img.alt = "logo";
  var h1 = document.createElement("h1");
  h1.innerHTML = card.cardNumber;
  var a = document.createElement("a");
  a.href = "#";
  a.innerHTML = "Delete";
  cardElement.appendChild(img);
  cardElement.appendChild(h1);
  cardElement.appendChild(a);
  cardElement.addEventListener("click", async () => {
    SelectedCard = card.cardId;
    cardElement.style.backgroundColor = "rgb(0, 0, 255, 1)";
    var cards = document.getElementsByClassName("cards");
    for (var i = 0; i < cards.length; i++) {
      if (cards[i] != cardElement) {
        cards[i].style.backgroundColor = "#4b6bfb";
      }
    }
  });
  return cardElement;
};

const FetchUserCards = async () => {
  const url =
    "http://localhost:5122/api/PaymentContorller/cards?userId=" +
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
    return;
  }
  const data = await res.json();
  if (data.length == 0) {
    hideLoadingScreen();
    document.getElementById("right-top").innerHTML = "<h1>No cards found</h1>";
    return;
  }
  data.forEach((card) => {
    constructCard(card).then((cardElement) => {
      document.getElementById("right-top").appendChild(cardElement);
    });
  });
  hideLoadingScreen();
};

const AddNewCard = async () => {
  var cardNumber = document.getElementById("card-num").value;
  var expDate = document.getElementById("card-exp").value;
  var cvv = document.getElementById("card-cvv").value;
  var userId = parseJwt(localStorage.getItem("token")).UserId;
  if (GetCardType(cardNumber) == "None") {
    spwanSnackBar("Invalid card number");
    return;
  }
  if (!/^(0[1-9]|1[0-2])\/\d{2}$/.test(expDate)) {
    spwanSnackBar("Invalid exp date");
    return;
  }
  if (!/^\d{3}$/.test(cvv)) {
    spwanSnackBar("Invalid CVV");
    return;
  }
  expDate = "20" + expDate.split("/")[1] + "-" + expDate.split("/")[0] + "-01";
  const date = new Date(expDate);
  const data = {
    userId: userId,
    cardNumber: cardNumber,
    expiryDate: date,
    cvv: cvv,
  };
  const url = "http://localhost:5122/api/PaymentContorller/addcard";
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
    return;
  }
  const response = await res.json();

  if (res.status == 200) {
    spwanSnackBar("Card added successfully", "green");
    setTimeout(() => {
      location.reload();
    }, 1000);
  } else {
    spwanSnackBar(response.message);
  }
};

const fetchBorrowedBooks = async () => {
  const url =
    "http://localhost:5122/api/Borrow/borrowed" +
    "?userId=" +
    parseJwt(localStorage.getItem("token")).UserId;
  const BorrowId = new URLSearchParams(window.location.search).get("borrowId");
  const res = await fetch(url, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + localStorage.getItem("token"),
    },
  });
  var flag = false;
  const data = await res.json();
  data.forEach((book) => {
    if (book.borrowId == BorrowId) {
      document.getElementById("fine-amount").innerHTML = "₹" + book.fine;
      flag = true;
    }
  });
  if (!flag) {
    window.location.href = "/UserDashboard.html";
  }
  hideLoadingScreen();
};

const PayFine = async () => {
  if (SelectedCard == -1) {
    spwanSnackBar("Please select a card");
    return;
  }
  const BorrowId = new URLSearchParams(window.location.search).get("borrowId");
  const url = "http://localhost:5122/api/PaymentContorller/payFine";
  const data = {
    userId: parseJwt(localStorage.getItem("token")).UserId,
    cardId: SelectedCard,
    borrowId: BorrowId,
  };
  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + localStorage.getItem("token"),
    },
    body: JSON.stringify(data),
  });
  const response = await res.json();
  if (res.status == 200) {
    spwanSnackBar("Fine paid successfully", "green");
    setTimeout(() => {
      window.location.href = "/UserDashboard.html";
    }, 1000);
  } else {
    spwanSnackBar(response.message);
  }
};

$(document).ready(function () {
  fetchBorrowedBooks();
  FetchUserCards();
});
