async function changeName() {
    try {
        let errorBlock = document.getElementById("errorBlock");
        let pass = document.getElementById("Pass").value;
        let name = document.getElementById('NewName').value;
        if (name.length >= 2) {
            let response = await fetch('/updName', {
                method: "POST", body: JSON.stringify(new function () {
                    this.Password = String(pass);
                    this.Name = String(name);
                })
            })
            if (response.ok) {
                location.reload();
            } else {
                errorBlock.innerText = "Incorrect password!";
                setTimeout(() => {
                    errorBlock.innerText = "";
                }, 5000)
            }
        } else {
            errorBlock.innerText = "Name must have 2 symbols and more!";
            setTimeout(() => {
                errorBlock.innerText = "";
            }, 5000)
        }
    } catch (exception) {
        location.reload();
    }
}

async function changePassword() {
    try {
        let errorBlock = document.getElementById("errorBlock");
        let pass = document.getElementById("lastPass").value;
        let newPass = document.getElementById('NewPassword').value;
        let newPassSecond = document.getElementById('NewPasswordSecond').value;
        if (newPass.length >= 8) {
            console.log(pass);
            if (newPass === newPassSecond) {
                let response = await fetch('/updPass', {
                    method: "POST", body: JSON.stringify(new function () {
                        this.LastPassword = String(pass);
                        this.NewPassword = String(newPass);
                    })
                })
                if (response.ok) {
                    location.reload();
                } else if (response.status === 412) {
                    errorBlock.innerText = "Incorrect password!";
                    setTimeout(() => {
                        errorBlock.innerText = "";
                    }, 5000)
                } else if (response.status === 418) {
                    errorBlock.innerText = "The password should be more complicated.";
                    setTimeout(() => {
                        errorBlock.innerText = "";
                    }, 5000);
                }
            } else {
                errorBlock.innerText = "Passwords don't match!"
            }
        } else {
            errorBlock.innerText = "Password must have 8 symbols and more!";
            setTimeout(() => {
                errorBlock.innerText = "";
            }, 5000)
        }
    } catch (exception) {
        location.reload();
    }
}