console.log(
  "%c" + "Hold Up!",
  "color: #7289DA; -webkit-text-stroke: 2px black; font-size: 72px; font-weight: bold;"
);
console.log(
  "%cThis is a browser feature for developers. If someone told you to copy/paste something here to enable a feature or hack someone's account, they scammed you!",
  "color: red; font-size: 24px;"
);
$(document).ready(function () {
  if (localStorage.getItem("token")) {
  }
});

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
      var introle =
        user["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      if (introle == "2") {
        document.getElementById("admin-dash-btn").style.visibility = "visible";
      }
    }
  } else if (window.location.href.includes("login.html") || window.location.href.includes("register.html") ||  window.location.href.includes("landing.html") ||window.location.href.includes("index.html")) {
    document.getElementById("dash-btn").innerHTML = "Login";
    document.getElementById("dash-btn").addEventListener("click", () => {
        window.location.href = "login.html";
        }) ; 

    hideLoadingScreen(); 
    return ; 
  }
    else {
        window.location.href = "login.html";
    }
});

document.getElementById("dash-btn").addEventListener("click", () => {
  window.location.href = "userDashboard.html";
});

document.getElementById("admin-dash-btn").addEventListener("click", () => {
  window.location.href = "AdminDashboard.html";
});
