// Write your JavaScript code.
console.log("hello javascript code .js");
//console.log("hello javascript code min.js");
/*
/* sign-in  
$("#signin-active").hide();
$("#signin-active").on("click", function(e) {
    console.log("hello submit button");
});

$("#sign-in-disabled").on("click", function(e) {
    console.log("hello disabled submit button");
    //$(".formtest #user-form span.name-req").show();
    //$(".formtest #user-form span.sem-req").show();
    // <span class="name-req"> Name is required my friend </span>
});
*/
/* -------------------------------- */

/* -------- Cart -------- */

if ($("#cart").length > 0)  {
    console.log("found cart");
    var allItemsInCart = [];
    $("#cart-items").html("<h4> Your cart is empty </h4>");
    $("#cart-table").hide();
    $("#cart #cart-loader").toggleClass("loader");
    for (var i = 0; i < localStorage.length; i++) {
        itemStoreId = localStorage.getItem(localStorage.key(i));
        key = localStorage.key(i);
        if(key.startsWith('itemId')) {
            if(itemStoreId !== 'INFO' && itemStoreId !== null && itemStoreId !== undefined) {
                singleItem = JSON.parse(itemStoreId);
                var item = { 
                    itemId: singleItem.itemId, 
                    quantity: singleItem.quantity, 
                    price: singleItem.price 
                };
                allItemsInCart.push(item); 
            }
        }
    }
    console.log("all items in the cart");
    console.log(allItemsInCart);
    var dataType = 'application/json; charset=utf-8';

    $.ajax({
        type: 'POST',
        url: '/Cart',
        dataType: 'json',
        contentType: dataType,
        data: JSON.stringify(allItemsInCart),
        success: function(result) {
            $("#cart #cart-loader").toggleClass("loader");
            console.log('Data received: ');
            console.log(result);
            var totalPrice = 0;
            var addItemData = "";
            var removeItemData = "";
            var quantityItemData = "";
            for(var j = 0; j < result.length; j++) {
                totalPrice = totalPrice + result[j].price * result[j].quantity;
                addItemData = "data-add="+ result[j].itemId;
                removeItemData = "data-remove="+ result[j].itemId;
                $("#cart-table").show();
                $('#cart-table tr:last').after(
                    "<tr>" + 
                        "<td>" + result[j].itemId + "</td>" +
                        "<td>" + result[j].title + "</td>" +
                        "<td>" + result[j].quantity + "</td>" +
                        "<td>" + result[j].price + "</td>" +  
                        "<td>" + "<button " + addItemData + " class=\"cart-add btn btn-success\"> add one </button>" + "</td>" + 
                        "<td>" + "<button " + removeItemData + " class=\"cart-remove btn btn-danger\"> remove one </button>" + "</td>" + 
                    "</tr>");
            }     
            
            getTotalPrice(totalPrice);
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            $("#cart #cart-loader").toggleClass("loader");
            $("#cart-items").html("<h4> Something went wrong. </h4>");
            console.log("Cart Post: Status: " + textStatus + " Error: " + errorThrown);
        } 
    });

    $(".buy-books").on("click", function(e) {
        console.log("buy all books");
        $.ajax({
            type: 'POST',
            url: '/Cart/Buy',
            dataType: 'json',
            contentType: dataType,
            data: JSON.stringify(allItemsInCart),
            success: function(result) {
                console.log('Data received: ');
                console.log(result);
                if(result === false) {
                    window.location.href = "http://localhost:5000/Account/Login";
                } else {
                    window.location.href = "http://localhost:5000/Checkout";
                }  
            },
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                console.log("Cart Post: Status: " + textStatus + " Error: " + errorThrown);
            } 
        });
    });
}

// add one item to the table
$("#cart-table").on('click', ".cart-add", function() {
    // put the localStorage key together
    var data = $(this).data("add");
    var itemToGet = "itemId"+data;
    // get the item and update it
    var item = localStorage.getItem(itemToGet);
    item = JSON.parse(item);
    item.quantity++;
    localStorage.setItem(itemToGet, JSON.stringify(item));
    $(this).parent().prev().prev().html(item.quantity);
    
    // get the total price to update it
    var totalPrice = $("#cart-items span").data("total");
    totalPrice = totalPrice + item.price;
    getTotalPrice(totalPrice);
});

$("#cart-table").on('click', ".cart-remove", function() {
    // get the item from localStorage and update the quantity
    var data = $(this).data("remove");
    var itemToGet = "itemId"+data; // put the localStorage key together
    var item = localStorage.getItem(itemToGet);
    item = JSON.parse(item);
    item.quantity--;

    // update the total price
    var totalPrice = $("#cart-items span").data("total");
    totalPrice = totalPrice - item.price;
    
    getTotalPrice(totalPrice);
    
    if(item.quantity === 0) {
        // remove one table row from the DOM (this is one <tr> that contains the item that we want to remove)
        $(this).parent().parent().remove();
        // remove the item from the localStorage 
        localStorage.removeItem(itemToGet); 
    } else {
        // update localStorage
        localStorage.setItem(itemToGet, JSON.stringify(item));
        // find item in the DOM and update the quantity 
        $(this).parent().prev().prev().prev().html(item.quantity);
    }
});

function getTotalPrice(total) {
    if(total === 0) {
        console.log("your cart is empty");
        $("#cart-table").hide();
        $("#cart-items").html("<h4> Your cart is empty </h4>");
    } else {
        var totalData = "data-total=" + total;
        $("#cart-items").html("<h4> Total price " + "<span " + totalData + ">" + total + "</span> ISK</h4>");
    }
}

/* -------------------------------- */

/* Home/Index - the frontpage */
$(".add-user-book").on("click", function(e) {
    //console.log("hello user add button");
    var bookId = $(this).data("book");
    var price = $(this).data("price");
    console.log("Logged in user adding: " + bookId + " and price: " + price);
    var item = { 
        itemId: bookId, 
        quantity: 1, 
        price: price 
    };
    
    var dataType = 'application/json; charset=utf-8';
    $.ajax({
        type: 'POST',
        url: '/Cart/LoggedInUserAdd',
        dataType: 'json',
        contentType: dataType,
        data: JSON.stringify(item),
        success: function(result) {

            console.log('Data received: ');
            console.log(result);
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log("Cart/LoggedInUserAdd Post: Status: " + textStatus + " Error: " + errorThrown);
        } 
    }); 
});

/* -------- Home/Index (frontpage) -------- */
$(".add-book").on("click", function(e) {
    var bookId = $(this).data("book");
    var storageKey = "itemId" + bookId; 
    var itemFromStorage = localStorage.getItem(storageKey);

    if(itemFromStorage) {
        console.log("I exist in the storage");
        //var item = localStorage.getItem(itemToGet);
        item = JSON.parse(itemFromStorage);
        item.quantity++;
        localStorage.setItem(storageKey, JSON.stringify(item));
        addItem(storageKey, itemFromStorage);
    } else {
        console.log("Create me please");
        var price = $(this).data("price");
        var bookToAdd = {};
        bookToAdd['itemId'] = bookId;
        bookToAdd['quantity'] = 1;
        bookToAdd['price'] = price;

        localStorage.setItem(storageKey, JSON.stringify(bookToAdd));
    }
});

// function to add items to localStorage
function addItem(key, item) {
    console.log("hello add book to localStorage");
    // 
}
