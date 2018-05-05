// Write your JavaScript code.
console.log("hello javascript code min.js");
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

$(".user-cart-item-remove").on("click", function(e) {
    // data -> useritemremove
    console.log("found logged in user item remove");

    //console.log("found logged in user item add");
    //var quantity = $("#cart-logged-in-user .user-item-quantity").data("quantity");
    //var price = $("#cart-logged-in-user .user-item-price").data("price");
    var quantity = $(this).parent().parent().prev().children(".user-item-quantity").data("quantity");
    var price = $(this).parent().parent().prev().children(".user-item-price").data("price");
    var bookId = $(this).data("useritemremove");
    console.log("Logged in user removing: " + bookId + " and price: " + price + " and quantity: " + quantity);
    
    var item = { 
        itemId: bookId, 
        quantity: quantity, 
        price: price,
        action: false // false is to remove one item
    };

    console.log(item);
    sendActionToCartController(item);
});

$(".user-cart-item-add").on("click", function(e) {
    // data -> useritemremove
    console.log("found logged in user item add");
    //var quantity = $("#cart-logged-in-user .user-item-quantity").data("quantity");
    //var price = $("#cart-logged-in-user .user-item-price").data("price");
    var quantity = $(this).parent().parent().prev().children(".user-item-quantity").data("quantity");
    var price = $(this).parent().parent().prev().children(".user-item-price").data("price");
    var bookId = $(this).data("useritemadd");
    console.log("Logged in user adding: " + bookId + " and price: " + price + " and quantity: " + quantity);
    
    var item = { 
        itemId: bookId, 
        quantity: quantity, 
        price: price,
        action: true // true is add one more
    };

    console.log(item);
    sendActionToCartController(item);
    //$(".add-user-book").on("click", function(e) {
        //console.log("hello user add button")
});
// logged in user cart operations
if($("#cart-logged-in-user").length > 0) {


    //$(".add-user-book").on("click", function(e) {
    //$("#cart-table").on('click', ".cart-add", function() {
    
    /*
    $(".user-cart-add") {
        // data -> useritemadd
    }
    */

};

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
                //$(".insert-cart-items").show();
                
                $(".grid-cart").append(
                    "<div class=grid-cart-item>" +
                        "<div class=grid-cart-item-title>" + result[j].title + "</div>" +
                        "<div class=grid-cart-item-price>" +
                            "<div>" + "Quantity: " + result[j].quantity + "</div>" +
                            "<div>" + "Price for each: " + result[j].price + "</div>" +
                        "</div>" +
                        "<div class=grid-cart-buttons>" +
                            "<div>" + "<button " + addItemData + " class=\"cart-add btn btn-success\"> + </button>" + "</div>" +
                            "<div>" + "<button " + removeItemData + " class=\"cart-remove btn btn-danger\"> - </button>" + "</div>" +
                        "</div>" +
                    "</div>");
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

function sendActionToCartController(item) {
    var dataType = 'application/json; charset=utf-8';

    if(!item && !item.itemId) {
        return;
    }
    else {
        $.ajax({
            type: 'POST',
            url: '/Cart/LoggedInUserCartAction',
            dataType: 'json',
            contentType: dataType,
            data: JSON.stringify(item),
            success: function(result) {
    
                console.log('Data received: ');
                console.log(result);
            },
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                console.log("Cart/LoggedInUserCartAction Post: Status: " + textStatus + " Error: " + errorThrown);
            } 
        }); 
    } 
}

// logged in user adds one book to the cart
$(".add-user-book").on("click", function(e) {
    //console.log("hello user add button");
    var bookId = $(this).data("book");
    var price = $(this).data("price");
    console.log("Logged in user adding: " + bookId + " and price: " + price);
    var item = { 
        itemId: bookId, 
        quantity: 1, 
        price: price,
        action: true
    };  
    sendActionToCartController(item);
});

/*
// logged in user removes one book from the cart
$(".cart-remove-user-book").on("click", function(e) {
    //console.log("hello user add button");
    var bookId = $(this).data("book");
    var price = $(this).data("price");
    console.log("Logged in user removes: " + bookId + " and price: " + price);
    var item = { 
        itemId: bookId, 
        quantity: 1, 
        price: price,
        action: 
    };  
    sendActionToCartController(item);
});
*/
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
