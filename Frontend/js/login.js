window.addEventListener("load", init);

function init() {
    const urlParameters = new URLSearchParams(window.location.search);
    const email = urlParameters.get('email');
    if (email !== null) {
        let emailField = document.getElementById("email");
        emailField.value = email;
    }

    let submitUser = document.getElementById('loginButton');
    submitUser.addEventListener("click", function(e){
        e.preventDefault();
        if (validationHandler()) {
            handleLoginUser();
        }
    });
}

function displayError(error, message)
{
    console.log(message);
    error.textContent = message;
    error.style.color = 'red';
    error.style.fontSize = '16px';
}

function validationHandler() {
    let uemail = document.getElementById("email").value;
    let upwd = document.getElementById("password").value;
    let formError = document.getElementById("formError");

    if (uemail === "" || upwd === "") {
        displayError(formError, 'Not all fields are filled in.');
        return false;
    } else {
        displayError(formError, '');
        return true;
    }
}

function handleLoginUser() {
    let uemail = document.getElementById("email").value;
    let upwd = document.getElementById("password").value;
    let formError = document.getElementById("formError");
    let url = 'https://localhost:5001/api/Authentication/token/';
    let user = {email: uemail, password: upwd};

    fetch(url,
        {
            method: "POST",
            body: JSON.stringify(user),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        })
        .then((response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                return null;
            }
        })
        .then((data) => {
            if(data == null){
                displayError(formError, "Wrong e-mail address and/or password.");
            } else {
                console.log(data.token);
                console.log(data);
                sessionStorage.setItem("token", data.token);
                window.open("waitingroom.html");
                window.close();
            }
        })
        .catch((error) => {
            console.log(error);
            displayError(formError, "Something went wrong. Please try again later.");
        })
}