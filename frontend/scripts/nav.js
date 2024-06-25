$(document).ready (function () {
    if (localStorage.getItem("token")) {
        handleTokenExpiry();
        handlePageLoad();
    }
});