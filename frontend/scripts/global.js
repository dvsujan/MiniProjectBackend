function parseJwt (token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

function spwanSnackBar(message, bgcolor = "red") {
  var x = document.getElementById("snackbar");
  x.innerHTML = "";
  if (bgcolor == "red") {
    x.innerHTML = "<i class='fas fa-exclamation-circle'></i> " + message;
    x.style.color = "#f44336";
    x.style.backgroundColor = "#F8F8FF";

  }
  else if (bgcolor == "green") {
    x.innerHTML = "<i class='fas fa-check-circle'></i> " + message;
    x.style.color = "#4CAF50";
    x.style.backgroundColor = "#F8F8FF";
  }
  else if (bgcolor == "blue") {
    x.innerHTML = "<i class='fas fa-info-circle'></i> " + message;
    x.style.color = "#2196F3";
    x.style.backgroundColor = "#F8F8FF";
  }
  x.className = "show";
  setTimeout(function () {
    x.className = x.className.replace("show", "");
  }, 3000);
}

const getBookDataFromGoogle = async (isbn) => {
  var res = await fetchWithTimeout(
    `https://www.googleapis.com/books/v1/volumes?q=isbn:${isbn}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    },
    5000
  );
  if (res === "timeout") {
    spwanSnackBar("Request timed out");
    return {
      thumbnail: "https://via.placeholder.com/150",
      description: "No description available",
      title: "Book Title",
    };
  }

  var data = await res.json();
  if(data.totalItems == 0){
    spwanSnackBar("Book not found ");
    return {
        error: true,
    };
  }

  var thumbnail = data.items[0].volumeInfo.imageLinks.thumbnail;
  var description = data.items[0].volumeInfo.description;
  var title = data.items[0].volumeInfo.title;
  var author = data.items[0].volumeInfo.authors;
  var category = data.items[0].volumeInfo.categories;
  var published = data.items[0].volumeInfo.publishedDate;
  var pageCount = data.items[0].volumeInfo.pageCount;
  var language = data.items[0].volumeInfo.language;
  var publisher = data.items[0].volumeInfo.publisher;
  return {
    thumbnail: thumbnail,
    description: description,
    title: title,
    author: author,
    category: category,
    published: published,
    pageCount: pageCount,
    language: language,
    publisher: publisher,
  };
};

function fetchWithTimeout(resource, options, timeout) {
    return  new Promise(async (resolve, reject) => {
        const controller = new AbortController();
        const id = setTimeout(() => controller.abort(), timeout);
        try {
            const response = await fetch(resource, {
                ...options,
                signal: controller.signal
            });
            resolve(response);
        } catch (error) {
            resolve("timeout");
        }
        clearTimeout(id);
    });
}

const showLoadingScreen = () => {
  document.getElementById("loading").style.display = "flex";
  document.getElementsByTagName("body")[0].style.overflow = "hidden";
}

const hideLoadingScreen = () => {
  document.getElementById("loading").style.display = "none"; 
  document.getElementsByTagName("body")[0].style.overflow = "auto";
}


