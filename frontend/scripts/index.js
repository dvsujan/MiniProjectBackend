$(document).ready(function () {
    initializePage();
});

const initializePage = () => {
    const url = new URL(window.location.href);
    const page = url.searchParams.get("page") || 1;
    document.getElementById("page").innerHTML = `${page}`;
    if (url.searchParams.get("search")) {
        const title = url.searchParams.get("search");
        loadSearchedBooks(title);
    } else {
        loadAllBooks();
    }
};

const getBookFromGoogle = async (isbn) => {
    const url = `https://www.googleapis.com/books/v1/volumes?q=isbn:${isbn}`;
    const res = await fetchWithTimeout(url, { method: "GET", headers: { "Content-Type": "application/json" } }, 5000);
    if (res === "timeout") return defaultGoogleData();

    const data = await res.json();
    return extractGoogleData(data);
};

const defaultGoogleData = () => ({
    thumbnail: "https://via.placeholder.com/150",
    description: "No description available",
    title: "Book Title",
});

const extractGoogleData = (data) => ({
    thumbnail: data.items[0].volumeInfo.imageLinks.thumbnail,
    description: data.items[0].volumeInfo.description,
    title: data.items[0].volumeInfo.title,
});

const fetchBooks = async (url) => {
    const res = await fetchWithTimeout(url, { method: "GET", headers: getHeaders() }, 5000);
    if (res === "timeout") {
        spwanSnackBar("Request timed out");
        return [];
    }
    return await res.json();
};

const getHeaders = () => ({
    "Content-Type": "application/json",
    Authorization: "Bearer " + localStorage.getItem("token"),
});

const loadSearchedBooks = async (title) => {
    const page = new URL(window.location.href).searchParams.get("page") || 1;
    const url = `http://localhost:5122/api/Book/search?title=${title}&page=${page}`;
    const data = await fetchBooks(url);
    renderBooks(data);
};

const loadAllBooks = async () => {
    const page = new URL(window.location.href).searchParams.get("page") || 1;
    const url = `http://localhost:5122/api/Book/all?page=${page}`;
    const data = await fetchBooks(url);
    renderBooks(data);
};

const renderBooks = async (data) => {
    const bookList = document.getElementById("all-books");
    bookList.innerHTML = "";
    showLoadingScreen();

    if (data.length === 0) {
        spwanSnackBar("No books found", "blue");
        hideLoadingScreen();
        return;
    }

    for (const book of data) {
        const googledata = await getBookFromGoogle(book.isbn);
        bookList.appendChild(createBookCard(book, googledata));
    }
    hideLoadingScreen();
};

const createBookCard = (book, googledata) => {
    const card = document.createElement("div");
    card.classList.add("card");
    card.appendChild(createCardImage(book.isbn));
    card.appendChild(createCardContent(book, googledata));
    return card;
};

const createCardImage = (isbn) => {
    const cardImage = document.createElement("div");
    cardImage.classList.add("card-image");
    const img = document.createElement("img");
    img.src = `https://covers.openlibrary.org/b/isbn/${isbn}-L.jpg`;
    img.alt = "Book cover";
    cardImage.appendChild(img);
    return cardImage;
};

const createCardContent = (book, googledata) => {
    const cardContent = document.createElement("div");
    cardContent.classList.add("card-content");
    cardContent.appendChild(createCardHeader(book, googledata));
    cardContent.appendChild(createCardSubtitle(googledata.description));
    cardContent.appendChild(createReadMoreButton(book.bookId));
    return cardContent;
};

const createCardHeader = (book, googledata) => {
    const cardHeader = document.createElement("div");
    cardHeader.classList.add("card-header");
    const title = document.createElement("h2");
    title.classList.add("card-title");
    title.innerHTML = book.title;
    cardHeader.appendChild(title);
    cardHeader.appendChild(createCardRating(book.rating, book.noOfRatings));
    return cardHeader;
};

const createCardRating = (rating, noOfRatings) => {
    const cardRating = document.createElement("div");
    cardRating.classList.add("card-rating");
    const stars = document.createElement("span");
    stars.classList.add("stars");
    stars.innerHTML = generateStars(rating);
    const ratingCount = document.createElement("span");
    ratingCount.classList.add("rating-count");
    ratingCount.innerHTML = `(${noOfRatings})`;
    cardRating.appendChild(stars);
    cardRating.appendChild(ratingCount);
    return cardRating;
};

const generateStars = (rating) => {
    let starstr = "";
    for (let i = 0; i < 5; i++) {
        starstr += i < rating ? "★" : "☆";
    }
    return starstr;
};

const createCardSubtitle = (description) => {
    const subtitle = document.createElement("p");
    subtitle.classList.add("card-subtitle");
    subtitle.innerHTML = description;
    return subtitle;
};

const createReadMoreButton = (bookId) => {
    const readMore = document.createElement("button");
    readMore.innerHTML = "Read More";
    readMore.onclick = () => window.location.href = `book.html?id=${bookId}`;
    return readMore;
};

const prevPage = () => navigatePage(-1);
const nextPage = () => navigatePage(1);

const navigatePage = (increment) => {
    const url = new URL(window.location.href);
    let page = parseInt(url.searchParams.get("page") || 1);
    page = Math.max(1, page + increment);
    const search = url.searchParams.get("search") ? `search=${url.searchParams.get("search")}&` : "";
    window.location.href = `index.html?${search}page=${page}`;
};

const searchBooks = () => {
    const title = document.getElementById("search-bar").value;
    if (title === "") {
        spwanSnackBar("Book title cannot be empty");
        return;
    }
    window.location.href = `index.html?search=${title}`;
};
