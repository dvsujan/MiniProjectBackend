const draw = () => {
  const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
  const randomData = () => Array.from({ length: 7 }, () => Math.floor(Math.random() * 40));
  return {
    labels: days,
    datasets: [
      { label: "Borrowed", data: randomData(), borderColor: "#0039a6", backgroundColor: "#0039a60A" },
      { label: "Returned", data: randomData(), borderColor: "#0076CE", backgroundColor: "#0076CE0A" },
    ],
  };
};

const createChart = (ctx) => {
  return new Chart(ctx, {
    type: "line",
    data: draw(),
    options: { scales: { y: { beginAtZero: true } } },
  });
};

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

const appendOptions = (data, select, editselect) => {
  data.forEach((location) => {
    const optionText = ` ( Floor: ${location.floor} shelf: ${location.shelf} )`;
    const createOption = (id, text) => {
      const option = document.createElement("option");
      option.value = id;
      option.text = text;
      return option;
    };

    select.appendChild(createOption(location.id, optionText));
    editselect.appendChild(createOption(location.id, optionText));
  });
};

const getLocations = async () => {
  const url = "http://localhost:5122/api/Book/locations";
  const res = await fetch(url, {
    method: "GET",
    headers: { "Content-Type": "application/json", Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
  const data = await res.json();
  appendOptions(data, document.getElementById("location-id"), document.getElementById("edit-location"));
};


const prepareBookData = (gogleBook, locationId, ISBN, stock) => {
  const formatDate = (date) => new Date(date).toISOString().split("T")[0];
  const getValue = (value, defaultValue) => (value === undefined ? defaultValue : value);

  return {
    title: gogleBook.title,
    authorName: getValue(gogleBook.author[0], "Unknown"),
    publisherName: getValue(gogleBook.publisher, "Unknown"),
    categoryName: getValue(gogleBook.category[0], "Unknown"),
    locationId: locationId,
    isbn: ISBN,
    stock: stock,
    publishedDate: formatDate(gogleBook.published),
  };
};

const addNewBook = async () => {
  const ISBN = document.getElementById("isbn").value;
  const stock = document.getElementById("stock").value;
  const locationId = document.getElementById("location-id").value;

  if (!ISBN || !stock || !locationId || stock < 0 || stock > 100) {
    spwanSnackBar("Please fill all fields and ensure stock is between 0 and 100");
    return;
  }

  try {
    const gogleBook = await getBookDataFromGoogle(ISBN);
    const data = prepareBookData(gogleBook, locationId, ISBN, stock);
    const url = "http://localhost:5122/api/Book/add";
    const res = await fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/json", Authorization: `Bearer ${localStorage.getItem("token")}` },
      body: JSON.stringify(data),
    });
    spwanSnackBar(res.status == 200 ? "Book added successfully" : (await res.json()).message, res.status == 200 ? "green" : undefined);
  } catch (err) {
    spwanSnackBar("Book not found");
  }
};

const searchBooks = async (query) => {
  const url = `http://localhost:5122/api/Book/search?title=${query}`;
  const res = await fetch(url, {
    method: "GET",
    headers: { "Content-Type": "application/json", Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
  return await res.json();
};

const displayDropdown = (data) => {
  const dropdown = document.getElementById("dropdown");
  dropdown.innerHTML = "";
  dropdown.style.display = "block";

  data.forEach((book) => {
    const button = document.createElement("button");
    button.type = "button";
    button.textContent = book.title;
    button.addEventListener("click", () => {
      document.getElementById("edit-title").value = book.title;
      document.getElementById("edit-Category").value = book.category;
      document.getElementById("dropdown").style.display = "none";
      editBook = book;
    });
    dropdown.appendChild(button);
  });
};

document.getElementById("edit-title").addEventListener("input", async function () {
  const query = this.value;
  if (query.length === 0) return;

  document.getElementById("loading").style.display = "block";
  const data = await searchBooks(query);
  document.getElementById("loading").style.display = "none";
  displayDropdown(data);
});

const updateBook = async () => {
  const title = document.getElementById("edit-title").value;
  const category = document.getElementById("edit-Category").value;
  const author = document.getElementById("edit-Author").value;
  const publisher = document.getElementById("edit-Publisher").value;
  const stock = document.getElementById("edit-stock").value;
  const location = document.getElementById("edit-location").value;

  if (!title || !category || !author || !publisher || !stock || !location || stock < 0 || stock > 100) {
    spwanSnackBar("Please fill all fields and ensure stock is between 0 and 100");
    return;
  }

  const sendData = {
    bookId: editBook.bookId,
    title: title,
    authorName: author,
    categoryName: category,
    publisherName: publisher,
    stock: stock,
    isbn: editBook.isbn,
    publishedDate: editBook.publishedDate,
    locationId: location,
  };

  const url = "http://localhost:5122/api/Book/update";
  const res = await fetch(url, {
    method: "PUT",
    headers: { "Content-Type": "application/json", Authorization: `Bearer ${localStorage.getItem("token")}` },
    body: JSON.stringify(sendData),
  });

  spwanSnackBar(res.status == 200 ? "Book updated successfully" : (await res.json()).message, res.status == 200 ? "green" : undefined);
};

const activateUser = async () => {
  const email = document.getElementById("email").value;
  if (!email) {
    spwanSnackBar("Please enter email");
    return;
  }

  const data = { email: email, isActive: true };
  const url = "http://localhost:5122/api/User/activate";
  const res = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json", Authorization: `Bearer ${localStorage.getItem("token")}` },
    body: JSON.stringify(data),
  });

  spwanSnackBar(res.status == 200 ? "User activated successfully" : (await res.json()).message, res.status == 200 ? "green" : undefined);
};

const fetchAnalytics = async (startDate, endDate) => {
  const url = `http://localhost:5122/api/Analytics`;
  const res = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json", Authorization: `Bearer ${localStorage.getItem("token")}` },
    body: JSON.stringify({ startDate: startDate.toISOString().split("T")[0], endDate: endDate.toISOString().split("T")[0] }),
  });
  return await res.json();
};

const updateChartData = (response,ctx) => {
  const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
  const borrowed = Array.from({ length: 7 }, () => 0);
  const returned = Array.from({ length: 7 }, () => 0);

  response.forEach((item) => {
    const day = new Date(item.date).getDay();
    borrowed[day] = item.booksBorrowed;
    returned[day] = item.booksReturned;
  });

  myChart.data.labels = days;
  myChart.data.datasets[0].data = borrowed;
  myChart.data.datasets[1].data = returned;
  myChart.update();
};

const GetAnalytics = async (ctx) => {
  const startDate = new Date();
  startDate.setDate(startDate.getDate() - 6);
  const endDate = new Date();
  const response = await fetchAnalytics(startDate, endDate);
  updateChartData(response, ctx);
};

$(document).ready(function () {
  const ctx = document.getElementById("analyticschart").getContext("2d");
  myChart = createChart(ctx);

  getLocations();
  GetAnalytics(ctx); 
  openCity(Event, "analytics");
});
