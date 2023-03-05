let userIsEntered = false;
let firstLoad = true;
async function GetPersonInformation() {
    let parentItem = document.getElementById("personInformation");
    let item = document.createElement("div");
        let response = await fetch('/getPersonInfo').catch();
        if (response.ok) {
            console.log("ent")
            userIsEntered = true;
            let user = JSON.parse(await response.text());
            item.innerHTML = `
            <p class="userData">
                <strong title="Name" class="userInfo" id="userName">${user.Name}</strong>
                <text title="CountMoney" class="userInfo" id="UserBalance">Balance: ${user.Balance}⚡</text>
                <button class="settingsBtn" onclick="changeBalance()" id="changeBalance">Put Money</button>
                <button class="settingsBtn" onclick="configure()" id="settings">Settings</button>
                <button class="settingsBtn" onclick="buyProducts()" id="buyProdcut">Buy Products</button>
                <text id="errorBlock"></text>
            </p>
            `;
            if (userIsEntered) {
                setInterval(() => {
                    updateBalanceOnPage();
                }, 15 * 1000);
            }
        } else 
        {
            console.log(11);
            userIsEntered = false;
            item.innerHTML = `
    <!--<form>-->
        <p class="enter">
            <input class="input_data" id="inputName" placeholder="Name">
            <input class="input_data" id="inputPassword" placeholder="Password"  type="password">
            <button class="enterBtn" onclick="register()" id="reg">Register</button>
            <button class="enterBtn" onclick="signIn()" id="singIn">Sign in</button>
            <text id="errorBlock"></text>
        </p>
<!--    </form>-->
        `;
        }
        if (firstLoad) {
            firstLoad = false;
            parentItem.appendChild(item);
            setTimeout(() => {
                updateBalanceOnPageWithReload(false);
            }, 1.5 * 1000);
        }
}
async function configure() {
    document.location.href="/settings";
}
async function changeBalance() {
    document.location.href="/balancePage";
}

async function buyProducts() {
    document.location.href="/buyProductsPage";
}


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

async function updateBalanceOnPageWithReload(needReload) {
    try {
        let balanceElement = document.querySelector('#UserBalance');
        balance = Number(balanceElement.textContent.replace('Balance: ', '').replace('⚡', ''));
        let response = await fetch('/getPersonInfo').catch();
        if (response.ok) {
            let user = JSON.parse(await response.text());
            if (balanceElement != null && user.Balance != balance) {
                balanceElement.textContent = 'Balance: ' + user.Balance + '⚡';
            }
        }
    } catch (exception) {
        if (needReload) {
            location.reload();
        }
    }

}