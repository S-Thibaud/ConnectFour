window.addEventListener("load", init);

function init() {
    let submitUser = document.getElementById('registerButton');
    submitUser.addEventListener("click", function(e) {
        e.preventDefault();
        if (validationHandler()) {
            handlePostUser();
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
    let upwdRepeat = document.getElementById("pwdRepeat").value;
    let uname = document.getElementById("nickname").value;
    let passwordErrorSpan = document.querySelector('#passwordErrorSpan');
    let formError = document.getElementById("formError");

    if (uemail === "" || upwd === "" || upwdRepeat === "" || uname === "") {
        displayError(formError, 'Not all fields are filled in.');
        return false;
    }

    if (upwd.length < 6) {
        displayError(formError, 'Password should be at least 6 characters long.');
        return false;
    }

    // Check that password and password repeat fields match
    if (upwd !== upwdRepeat) {
        displayError(formError, 'Passwords do not match.');
    } else {
        displayError(formError, '');
        return true;
    }
}

function handlePostUser() {
    let uemail = document.getElementById("email").value;
    let upwd = document.getElementById("password").value;
    let upwdRepeat = document.getElementById("pwdRepeat").value;
    let uname = document.getElementById("nickname").value;
    let url = 'https://localhost:5001/api/Authentication/register/';
    let user = {email: uemail, password: upwd, nickName: uname};
    let formError = document.getElementById("formError");

    fetch(url,
    {
        method: "POST",
        body: JSON.stringify(user),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        }
    })
    .then((response) => {
        if (response.status === 200) {
            // als het gelukt is
            window.open(`index.html?email=${uemail}`);
            window.close();
        } else {
            return response.json();
        }})
    .then((data) => {
        //console.log(data.message);
        displayError(formError, data.message);
    })
    .catch((error) => {
        console.log(error);
        displayError(formError, "Something went wrong. Please try again later.");
    })
}