const GetDetails = async(e)=>{
    const email = document.getElementById("register-email").value;
    const password = document.getElementById("register-password").value;
    const confPassword = document.getElementById("register-cpassword").value;
    const username = document.getElementById("register-username").value;
    return { 
        email: email,
        password: password,
        confPassword: confPassword,
        username: username
    }

}; 

const ValidateDetails = async (details)=>{
    if (details.email == "" || details.password == "" || details.confPassword == "" || details.username == ""){
        spwanSnackBar("Please fill all fields");
        return false;
    }
    const emailRegex = /\S+@\S+\.\S+/;
    if (!emailRegex.test(details.email)) {
        spwanSnackBar("Invalid email address");
        return false;
    }
    if(details.username.length < 3){
        spwanSnackBar("Username must be at least 3 characters");
        return false;
    }
    if (details.password.length < 6) {
        spwanSnackBar("Password must be at least 6 characters");
        return false;
    }
    if (details.password != details.confPassword){
        spwanSnackBar("Passwords do not match");
        return false;
    }
    return true;

}

const RegisterUser = async(e)=>{ 
  e.preventDefault();
  const details = await GetDetails(e);
  if (!await ValidateDetails(details)){
      return;
  }
    const data = {
        email: details.email,
        password: details.password,
        username: details.username
    };
    const url = "http://localhost:5122/api/User/register"; 
    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
    });
    if (response.status == 200) {
        spwanSnackBar("User registered successfully", "green");
    } else {
        spwanSnackBar((await response.json()).message);
    }
}

