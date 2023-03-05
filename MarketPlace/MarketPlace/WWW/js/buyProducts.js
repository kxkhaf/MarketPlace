async function addProducts(information) {
    let productItem = document.getElementById("productItemCollector");
    let item = document.createElement("div");
    item.innerHTML = `
    <div class="productItem" style="top: calc( 5 * (1vmin + 1vmax));">
        <strong title="Your products" style="
        flex-direction: column;
        text-align: center;
        white-space: nowrap;
        top: 2%;
        -webkit-text-stroke: calc(0.01 * (1vw + 2vh)) #ddcd02;
        font-family: Luminari, fantasy;
        text-overflow: ellipsis;
        position: absolute;
        font-size: calc(2 * (1vmin + 1vmax));
        left: 15%;
        ">Shopping list</strong>
        <textarea title="${information}" class="productInfo" id="productInfo" maxlength="250" readonly>${information}</textarea>
        <button class="buyButton" onclick="buyAllProducts()">Buy!</button>
    </div>
    `;
    productItem.appendChild(item);
}
//let price = 0;
async function getProductList() {
    let loadingElement = document.querySelector("#loadingElement");
    setTimeout(() => {
        loadingElement.remove();
        fetch('/getUserProductsList').then(result => result.json()).then(products => {
        addProducts(products.ShoppingList);
        //price = products.Price;
    });
    }, 1000)
}

function ProductList(shoppingList, price) {
    this.ShoppingList = shoppingList;
    this.Price = price;
}

async function buyAllProducts() {
    let productInfo = document.getElementById("productInfo");
    let errorBlock = document.getElementById("errorBlock");
    let response = await fetch("/buyAllProducts");
    if (response.ok) {
        
        productInfo.value = "THANKS FOR BUYING!"
/*        let balanceElement = document.querySelector('#UserBalance');
        if (balanceElement != null){
            balance = Number(balanceElement.textContent.
            replace('Balance: ', '').replace('⚡', ''));
        }
        balanceElement.textContent = "Balance: " + (balance - price) + "⚡";*/
        setTimeout(() => {
            location.reload();
        }, 5000)
    } else {
        errorBlock.innerText = "You don't have enough money!"
        setTimeout(() => {
            errorBlock.innerText = "";
        }, 5000)
    }
    
}