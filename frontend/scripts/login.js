function parseJwt (token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

$(document).ready(function () {
  if (localStorage.getItem("token")) {
    var token = localStorage.getItem("token");
    var user = parseJwt(token);
    var now = new Date();
    var expiry = new Date(user.exp * 1000);
    if (now > expiry) {
      localStorage.removeItem("token");
      window.location.href = "login.html";
    } else {
        var introle = user['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']; 
        if(introle == "2"){
            window.location.href = "AdminDashboard.html";
        }

    }
  }
});


async function login(e) {
  e.preventDefault();
  var email = document.getElementById("email").value;
  var password = document.getElementById("password").value;
  if (email == "" || password == "") {
    spwanSnackBar("Please fill all fields");
    return;
  }
  var emailRegex = /\S+@\S+\.\S+/;
  if (!emailRegex.test(email)) {
    spwanSnackBar("Invalid email address");
    return;
  }
  if (password.length < 6) {
    spwanSnackBar("Password must be at least 6 characters");
    return;
  }
  var data = {
    email: email,
    password: password,
  };
  const loginUrl = "http://localhost:5122/api/User/login";
  var res = await fetch(loginUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });
  var response = await res.json();
  console.log(response);
  if (res.status == 200) {
    console.log(response);
    localStorage.setItem("token", response.token);
    window.location.href = "index.html";
  } else {
    spwanSnackBar(response.message);
  }
}
