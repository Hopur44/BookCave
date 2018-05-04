// Write your JavaScript code.
console.log("hello javascript code .js");

console.log("hello javascript code min.js");
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
    if(localStorage.length !== 0) {
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

                //var tableRow = "";
                var title = "";
                var totalPrice = 0;
                var id = 0;
                var price = 0;
                var addItemData = "";
                var removeItemData = "";
                var quantityItemData = "";
                var quantity = 0;
                for(var j = 0; j < result.length; j++) {
                    //tableRowId = "id=tableRow" + j;
                    id = result[j].itemId;
                    title = result[j].title;
                    quantity = result[j].quantity;
                    price = result[j].price;
                    totalPrice = totalPrice + price * quantity;
                    addItemData = "data-add="+ id;
                    removeItemData = "data-remove="+ id;
                    $("#cart-table").show();
                    $('#cart-table tr:last').after(
                        "<tr>" + 
                            "<td>" + id + "</td>" +
                            "<td>" + title + "</td>" +
                            "<td>" + quantity + "</td>" +
                            "<td>" + price + "</td>" +  
                            "<td>" + "<button " + addItemData + " class=\"cart-add btn btn-success\"> add one </button>" + "</td>" + 
                            "<td>" + "<button " + removeItemData + " class=\"cart-remove btn btn-danger\"> remove one </button>" + "</td>" + 
                        "</tr>");
                }
                
                //console.log(" total price: " + totalPrice);
                var totalData = "data-total=" + totalPrice;
                $("#cart-items").html("<h4> Total price " + "<span " + totalData + ">" + totalPrice + "</span> ISK</h4>");
                //$('#cart-table tr:last').after("<tr><td>"+ result[0].itemId + "</td><td>" + result[0].quantity + "</td></tr>");
            },
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                $("#cart #cart-loader").toggleClass("loader");
                $("#cart-items").html("<h4> Something went wrong. </h4>");
                console.log("Cart Post: Status: " + textStatus + " Error: " + errorThrown);
            } 
        });
    } else {
        console.log("localStorage is empty")
    }
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

    // get the total price to update it
    var totalPrice = $("#cart-items span").data("total");
    totalPrice = totalPrice + item.price;
    var totalData = "data-total=" + totalPrice;
    //var itemQuantity = "#item-quantity" + item.itemId;
    console.log("---------------------")
    $(this).parent().prev().prev().html(item.quantity);
    //console.log(item.quantity);
    //console.log($(itemQuantity));
    //$(itemQuantity).html(item.quantity);
    $("#cart-items").html("<h4> Total price " + "<span " + totalData + ">" + totalPrice + "</span> ISK</h4>");
});

// remove one item from table 
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
    
    if(totalPrice === 0) {
        console.log("your cart is empty");
        $("#cart-table").hide();
        $("#cart-items").html("<h4> Your cart is empty </h4>");
    } else {
        var totalData = "data-total=" + totalPrice;
        $("#cart-items").html("<h4> Total price " + "<span " + totalData + ">" + totalPrice + "</span> ISK</h4>");

    }
    
    if(item.quantity === 0) {
        // remove one table row from the DOM (this is one <tr> that contains the item that we want to remove)
        $(this).parent().parent().remove();
        // remove the item from the localStorage 
        localStorage.removeItem(itemToGet); 
    } else {
        // update localStorage
        localStorage.setItem(itemToGet, JSON.stringify(item));
        // find item in the DOM and update the quantity 
        //var itemQuantity = "#item-quantity" + item.itemId; 
        //$(itemQuantity).html(item.quantity);
        $(this).parent().prev().prev().prev().html(item.quantity);
    }
});


/* -------------------------------- */
// <button class="add-book" data-book=@book.Id data-price=@book.Price> add me</button>
/* Home/Index - the frontpage */
// bæta einhverju id við div-ið sem heldur utan um þetta..

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
