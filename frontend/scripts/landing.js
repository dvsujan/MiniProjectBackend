


const searchBooks = ()=>{ 
    const title = document.getElementById("search-bar").value;
    if (!title) {
        spwanSnackBar("Book title cannot be empty");
        return;
    }
    window.location.href = `index.html?search=${title}`;
}