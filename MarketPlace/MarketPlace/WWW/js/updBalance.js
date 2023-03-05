let isSending = false;

async function updBalance(count, timeSecond) {
    if (!isSending) {
        isSending = true;
        balanceElement = document.querySelector('#UserBalance');
        let balance = Number(balanceElement.textContent.replace('Balance: ', '').replace('⚡', ''));
        //setTimeout(() => {
            let response = fetch('/updBalance', {method: "POST", body: JSON.stringify(count)}).then(() => {
                if (balanceElement != null) {
                    balanceElement.textContent = "Balance: " + (balance + count) + "⚡";
                    document.querySelector(".addBal").disabled = false;
                    isSending = false;
                }
            })
        //}, timeSecond * 1000)
        document.querySelector(".addBal").disabled = true;
        await makeLoading();
    }
}

async function makeLoading() {
        setTimeout(() => {
            if (isSending) {
                document.querySelector('#UserBalance').textContent = "Balance: LOADING";
                setTimeout(() => {
                    if (isSending) {
                        document.querySelector('#UserBalance').textContent = "Balance: LOADING.";
                        setTimeout(() => {
                            if (isSending) {
                                document.querySelector('#UserBalance').textContent = "Balance: LOADING..";
                                setTimeout(() => {
                                    if (isSending) {
                                        document.querySelector('#UserBalance').textContent = "Balance: LOADING...";
                                        console.log(isSending);
                                        if (isSending) {
                                            makeLoading(isSending);
                                        }
                                    }
                                }, 200)
                            }
                        }, 225)
                    }
                }, 250)
            }
        }, 325)

}

async function getProducts() {
    /*let result = await (await fetch('/getProductsFromDB')).text();
    productsOnDB = JSON.parse(result);
    for (let i = 0; i < productsOnDB.length; i++) {
        await addProductItemWithIndex(productsOnDB[i].Name, productsOnDB[i].Information, productsOnDB[i].Rating, productsOnDB[i].Count, productsOnDB[i].Id, productsOnDB[i].Price);
    }
    setTimeout(() => {
        balanceElement = document.querySelector('#UserBalance');
        if (balanceElement != null){
            balance = Number(balanceElement.textContent.
            replace('Balance: ', '').replace('⚡', ''));
            console.log(balance);
        }
    }, 500)*/
}
