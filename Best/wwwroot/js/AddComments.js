// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const addCommentJson = (id, UserId, Text) => {
    console.log("clicked");
    $.ajax({
        type: "POST",
        url: "/Posts/AddComment",
        data: {
            "Id": id,
            "UserId": UserId,
            "Text": Text.val()
        }
    }).done((res) => {
        if (res) {
            var fullComments = document.createElement("div");
            fullComments.classList.add("card");
            fullComments.classList.add("m-1");
            fullComments.classList.add("d-flex");
            fullComments.classList.add("flex-row");

            var siderAvarat = document.createElement("div");
            siderAvarat.classList.add("col-1");
            siderAvarat.innerHTML = document.getElementById("leftCard").innerHTML;

            var rightside = document.createElement("div");
            rightside.classList.add("w-100");

            var cardHead = document.createElement("div");
            cardHead.classList.add("card-header");
            var today = new Date();
            cardHead.innerHTML = document.getElementById("cardHead").innerHTML + " " + today.getDate() + "." + today.getMonth() + "." + today.getFullYear() + " " + today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();

            var cardbody = document.createElement("div");
            cardbody.classList.add("card-body");
            cardbody.innerText = Text.val();

            rightside.append(cardHead);
            rightside.append(cardbody);

            fullComments.append(siderAvarat);
            fullComments.append(rightside);

            $("#comments").append(fullComments); 
        }
        else {
            alert("Sorry. Comment dont add in DB");
        }
    });
    return false;
}