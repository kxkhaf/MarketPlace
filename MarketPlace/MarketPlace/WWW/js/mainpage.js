async function register() {
    let nameEl = document.getElementById("inputName");
    let passwordEl = document.getElementById("inputPassword");
    let errorBlock = document.getElementById("errorBlock");
    if (passwordEl.value.length < 8) {
        errorBlock.innerText = "The password must contain 8 characters or more!";
        setTimeout(() => {
            errorBlock.innerText = "";
        }, 3000);
    }
    else {
        let response = await fetch("/register", { method: "POST", body: JSON.stringify(new User(0,nameEl.value, passwordEl.value))});
        let responseText = await response.text();
        if (responseText === "All done!") {
            setTimeout(() => {
                errorBlock.innerText = "";
                document.location.href = "http://localhost:1111/products";
            }, 500);
            errorBlock.innerText = "You have successfully registered!";
        }
        else {
            errorBlock.innerText = "Invalid user name or such user name already exists. \n" +
                "The length of the name must be 2 or more. \n" +
                "The password should be more complicated.";
            setTimeout(() => {
                errorBlock.innerText = "";
            }, 3000);
        }
    }
}

async function signIn() {
    let nameEl = document.getElementById("inputName");
    let passwordEl = document.getElementById("inputPassword");
    let errorBlock = document.getElementById("errorBlock");
    if (passwordEl.value.length < 8) {
        errorBlock.innerText = "The password must contain 8 characters or more!";
        setTimeout(() => {
            errorBlock.innerText = "";
        }, 3000);
    }
    else {
        let response = await fetch("/signIn", { method: "POST", body: JSON.stringify(new User(0,nameEl.value, passwordEl.value))});
        let responseText = await response.text();
        if (responseText === "All done!") {
            errorBlock.innerText = "You have successfully entered!";
            document.location.href ="http://localhost:1111/products";
        }
        else{
            errorBlock.innerText = "Incorrect data!";
            setTimeout(() => {
                errorBlock.innerText = "";
            }, 3000);
        }
    }
}

function User(id = 0, name, password, balance = 0){
    this.Id = id;
    this.Name = name;
    this.Password = password;
    this.Balance = balance;
}

async function redirectProducts(){
    document.location.href("/products");
}
async function redirectProductsNotRegister(){
    document.location.href="http://localhost:1111/productsNotRegistered"}

async function updateBalanceOnPage() {
    try {
        let balanceElement = document.querySelector('#UserBalance');
        balance = Number(balanceElement.textContent.
        replace('Balance: ', '').replace('⚡', ''));
        let response = await fetch('/getPersonInfo').catch();
        if (response.ok) {
            let user = JSON.parse(await response.text());
            if (balanceElement != null  && user.Balance != balance){
                balanceElement.textContent = 'Balance: ' + user.Balance + '⚡';
            }
        }
    }
    catch (exception) {
        location.reload();
    }
}



