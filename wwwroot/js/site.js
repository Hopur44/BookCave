// Write your JavaScript code.
console.log("javascript code .js");

//var totalCartQuantity = 0;
//var userTotalCartQuantity = 0;
/* -------- Header -------- */
// cart-header

// All Controllers
// All users
if ($(".cart-header").length > 0)  {
    console.log("cart header");
    //console.log($(".user-cart-header"));
    console.log($(".cart-header"));

    // localStorage til að sækja total
    var total = 0;
    var items = getAllItemsFromLocalStorage();
    for(var i = 0; i < items.length; i++) {
        total = total + items[i].quantity;
    }
    //console.log("total items from localStorage: " + total);
    //$(".cart-header").attr('data-amount', total);

    updateCartTotal(total)
}

if ($(".user-cart-header").length > 0)  {
    console.log("user cart header");
    console.log($(".user-cart-header"));
    //console.log($(".user-cart-header").text("Cart: " + 5 + " items"));

    //console.log("did I go here?");
    // ajax til að sækja total cart items....

    getTotalUserCartQuantity();
    
    
}

function getTotalUserCartQuantity() {
    //url: '/Home/AllReviews',
    //var dataType = 'application/json; charset=utf-8';
    $.ajax({
        type: 'GET',
        url: '/Cart/GetTotalQuantity',
        //dataType: 'json',
        //contentType: dataType,
        //data: JSON.stringify(1),
        success: function(result) {
            console.log('Data received: ');
            console.log(result);
            updateUserCartTotal(result);
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log("Cart Post: Status: " + textStatus + " Error: " + errorThrown);
            updateUserCartTotal(0);
        } 
    });
}
/* -------- Cart -------- */

// Controller/Cart/Index
// logged in user 
// adds one more book to his cart
$(".user-cart-item-add").on("click", function(e) {    
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
    
    console.log("user-add-data");
    console.log($(".user-cart-header").append("<div> hello </div>"));
    
    var amount = $(".user-cart-header").data("amount");
    console.log("amount: " + amount);

    sendActionToCartController(item);
    alert("One item of BookTitle has been added to your cart");
    
    // we update the total cart items in the header
    var userTotalCartQuantity = $(".user-cart-header").data("amount");
    userTotalCartQuantity++;
    updateUserCartTotal(userTotalCartQuantity); 
    
    // we update the item quantity on the screen 
    updateQuantityHtml(this, quantity+1);
    // we update the total price on the screen
    updateCartTotalPrice(item.price, true); // true is add price to totalPrice

});

/*
function updateTotalQuantity(quantity) {
    console.log("update total quantity");
    console.log($(".user-cart-header"));
    console.log($(".cart-header"));

}
*/

// Controller/Cart/Index
// logged in user
// we get the quantity for book
function getUserQuantity(_this) {
    return ($(_this).parent().parent().prev().children(".user-item-quantity").data("quantity"));
}

// Controller/Cart/Index
// logged in user
// we get the price for book
function getUserPrice(_this) {
    return ($(_this).parent().parent().prev().children(".user-item-price").data("price"));
}

// Controller/Cart/Index
// logged in user
// we get the id for book
function getUserBookId(_this, action) {
    return ($(_this).data(action))
}

// Controller/Cart/Index
// logged in user
// we create object that we will send to the controller
// actionToTake is bool, true is to add one, false is to remove one
function createItemForAjax(bookId, quantity, price, actionToTake) {
    return (
        item = { 
            itemId: bookId, 
            quantity: quantity, 
            price: price,
            action: actionToTake // false is to remove one item from the database
        }
    );
}

// Controller/Cart/Index
// logged in user 
// removes one book from his cart
$(".user-cart-item-remove").on("click", function(e) {
    var quantity = getUserQuantity(this); //$(this).parent().parent().prev().children(".user-item-quantity").data("quantity");
    var price = getUserPrice(this);//$(this).parent().parent().prev().children(".user-item-price").data("price");
    var bookId = getUserBookId(this, "useritemremove"); //$(this).data("useritemremove");
    console.log("Logged in user removing: " + bookId + " and price: " + price + " and quantity: " + quantity);
    var item = createItemForAjax(bookId, quantity, price, false);
    
    // we send the action to the controller to remove one item
    sendActionToCartController(item);
    alert("One item of BookTitle has been removed from your cart ");
    
    // we update the total cart items in the header
    var userTotalCartQuantity = $(".user-cart-header").data("amount");
    userTotalCartQuantity--;
    updateUserCartTotal(userTotalCartQuantity);

    // we update in totalPrice on the screen 
    updateCartTotalPrice(item.price, false); // false is deduct price from totalPrice
    // if quantity is going to be zero then we remove this row from the DOM
    if(quantity - 1 === 0 ) {
        //console.log("zero");
        $(this).parent().parent().parent().remove();
    } else {
        updateQuantityHtml(this, quantity-1);  
    } 
});

// Controller/Cart/Index
// logged in user 
// clears the cart of book with this Id
$(".user-cart-item-clear").on("click", function(e) {

    console.log("Logged in user clearing: " + bookId + " from his cart");
    var quantity = getUserQuantity(this); 
    var price = getUserPrice(this);
    var bookId = getUserBookId(this, "useritemclear");
    var item = createItemForAjax(bookId, 1, price, false); 
    //console.log(item);
    
    // we send the action to the controller to remove one item
    sendActionToCartController(item);
    alert("We removed BookTitle from your cart");
    
    // we update the total cart items in the header
    var userTotalCartQuantity = $(".user-cart-header").data("amount");
    userTotalCartQuantity = userTotalCartQuantity - quantity;
    updateUserCartTotal(userTotalCartQuantity);

    // we update in totalPrice on the screen 
    updateCartTotalPrice(price * quantity, false); // false is deduct price from totalPrice
    updateQuantityHtml(this, quantity-1);  
    // we remove this row from the cart / DOM
    $(this).parent().parent().parent().remove();  
});

// Controller/Cart/Index
// logged in user 
// clearing all the items from the cart, now the cart is empty
$(".user-cart-item-clear-all").on("click", function(e) {
    console.log("Controller/Cart/Index - Logged in user clearing his cart");
    // remove the rows from the screen
    $("#cart-logged-in-user .grid-cart").remove(); 
    // we set the price to be 0
    setTotalPrice(0);
    updateUserCartTotal(0);
    //send ajax request to the controller/Cart/ClearCart to remove all
    // alert("we cleared your cart");

    //Cart/LoggedInUserCartClear

    var dataType = 'application/json; charset=utf-8';
    $.ajax({
        type: 'POST',
        url: '/Cart/LoggedInUserCartClear',
        dataType: 'json',
        contentType: dataType,
        data: JSON.stringify(1),
        success: function(result) {
            console.log('Data received: ');
            console.log(result);
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log("Cart Post: Status: " + textStatus + " Error: " + errorThrown);
        } 
    });
});

// Controller/Cart/Index
// not logged in user
// we append cart items to the DOM, this is one row in the cart
function appendAllCartItems(item) {
    var totalPrice = totalPrice + item.price * item.quantity;
    var addItemData = "data-add="+ item.itemId;
    var removeItemData = "data-remove="+ item.itemId;
    var clearItemData = "data-removeall="+ item.itemId;
    $(".grid-cart").append(
        "<div class=grid-cart-item>" +
            "<div class=grid-cart-item-title>" + item.title + "</div>" +
            "<div class=grid-cart-item-price>" +
                "<div class=cart-quantity>" + "Quantity: " + item.quantity + "</div>" +
                "<div>" + "Price for each: " + item.price + "</div>" +
            "</div>" +
            "<div class=grid-cart-buttons>" +
                "<div>" + 
                    "<button " + addItemData + " class=\"cart-add btn btn-default fas fa-plus\"></button>" + 
                "</div>" +
                "<div>" + 
                    "<button " + removeItemData + " class=\"cart-remove btn btn-default fas fa-minus\"></button>" + 
                "</div>" +
                "<div>" + 
                    "<button " + clearItemData + " class=\"cart-clear-item btn btn-default fa fa-trash\"></button>" + 
                "</div>" +
            "</div>" +
        "</div>");
}

// we get all the books from the localStorage
function getAllItemsFromLocalStorage() {
    var allItemsInCart = [];
    for (var i = 0; i < localStorage.length; i++) {
        itemStoreId = localStorage.getItem(localStorage.key(i));
        key = localStorage.key(i);
        if(key.startsWith('itemId')) {
            if(itemStoreId !== 'INFO' && itemStoreId !== null && itemStoreId !== undefined) {
                singleItem = JSON.parse(itemStoreId);
                var item = { 
                    itemId: singleItem.itemId, 
                    quantity: singleItem.quantity, 
                    price: singleItem.price,
                    title: singleItem.title
                };
                totalPrice = totalPrice + singleItem.price * singleItem.quantity;
                allItemsInCart.push(item);
            }
        }
    }
    return (allItemsInCart);
}

// Controller/Cart/Index
// not logged in user
// get all cart items and append them to the cart (DOM)
if ($("#cart").length > 0)  {

    var allItems = getAllItemsFromLocalStorage();
    console.log("Controller/Cart/Index - not logged in user - getting all items from localStorage");
    console.log(allItems);

    $("#cart-items").html("<h4> Your cart is empty </h4>");
    var addItemData = ""; 
    var removeItemData = ""; 
    var clearItemData = "";
    var totalPrice = 0;

    for (var j = 0; j < allItems.length; j++) {
        totalPrice = totalPrice + allItems[j].price * allItems[j].quantity;
        appendAllCartItems(allItems[j]);
    }

    setTotalPrice(totalPrice);
    //console.log("all items in the cart");
    //console.log(allItemsInCart);

    $(".buy-books").on("click", function(e) {
        console.log("buy all books");
        window.location.href = "http://localhost:5000/Account/Login";
    });
}

// Controller/Cart/Index
// logged in user 
// This user cart is empty
if ($("#logged-in-empty-cart").length > 0)  {
    console.log("not implemented yet - found logged in user with empty cart");
    console.log(" /Controller/Cart/Index - logged in user your - with empty cart");
     // we found empty cart for logged in user

    // check localStorage if we find something there
        // then we ask the user if he wants to load the items that he/ or someone added on this device
        // to the database?
    var allItemsFromLocalStorage = getAllItemsFromLocalStorage();
    if(allItemsFromLocalStorage.length > 0) {
        console.log("we found items that were added on this device when you were not logged in");
        console.log(allItemsFromLocalStorage);

        var dataType = 'application/json; charset=utf-8';
        
        $.ajax({
            type: 'POST',
            url: '/Cart/AddAllCartItems',
            dataType: 'json',
            contentType: dataType,
            data: JSON.stringify(allItemsFromLocalStorage),
            success: function(result) {
                console.log('Data received: ');
                console.log(result);
                localStorage.clear();
            },
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                console.log("Cart/LoggedInUserCartAction Post: Status: " + textStatus + " Error: " + errorThrown);
            } 
        });

        //AddAllCartItems
        console.log("do you want us to add them to your cart?");
    } else {
        console.log("your localStorage cart is empty");
    }


        // if yes send everything with ajax 
        // and re-direct the user to cart.
}

// Controller/Cart/Index
// we set the total price 
function setTotalPrice(totalPrice) {
    if(totalPrice === 0) {
        console.log("your cart is empty");
        $("#cart-items").html("<h4> Your cart is empty </h4>");
        $(".clear-cart").hide(); // we hide the clear cart button
        $(".buy-books").hide(); // we hide the buy books button
        $(".user-clear-cart").hide(); // we hide the logged in user clear cart button
    } else {
        var totalData = "data-total=" + totalPrice;
        $("#cart-items").html("<h4> Total price " + "<span " + totalData + ">" + totalPrice + "</span> ISK</h4>");
    }
}

// Controller/Cart/Index
// we update the total price
function updateCartTotalPrice(price, actionAddOrRemove) {
    var totalPrice = $("#cart-items span").data("total");  
    // if true then we are adding, false we deduct
    if(actionAddOrRemove === true) {
        totalPrice = totalPrice + price;
    } else {
        totalPrice = totalPrice - price;
    }
    console.log("totalprice:" + totalPrice);
    setTotalPrice(totalPrice); // we change the total price on the screen using DOM operations
}

// Controller/Cart/Index
// we update quantity of items in the cart
function updateQuantityHtml (_this, quantity) {
    // update the data-set -> .data("key". variable)
    $(_this).parent().parent().prev().children(".user-item-quantity").data("quantity", quantity)
    // update the text on the screen 
    $(_this).parent().parent().prev().children(".user-item-quantity").text("Quantity: " + quantity); //.html("Quantity: " + quantity-1));
}

// logged in user updates the total cart items
function updateUserCartTotal(totalCartQuantity) {
    $(".user-cart-header").data("amount", totalCartQuantity);
    $(".user-cart-header").attr('data-amount', totalCartQuantity);
    $(".user-cart-header").text("Cart: " + totalCartQuantity + " items");
}

// not logged in user updates the total cart items
function updateCartTotal(cartTotalQuantity) {
    $(".cart-header").data("amount", cartTotalQuantity);
    $(".cart-header").attr('data-amount', cartTotalQuantity);
    $(".cart-header").text("Cart: " + cartTotalQuantity + " items");
}
// Controller/Cart/Index
// not logged in user
// add one item to the cart
$(".grid-cart").on('click', ".cart-add", function() {
    
    var cartTotalQuantity = $(".cart-header").data("amount");
    cartTotalQuantity++;
    updateCartTotal(cartTotalQuantity);
    // put the localStorage key together
    var itemToGet = getLocalStorageKey(this, "add");
    console.log("/Cart/Index - not logged in - adding one more book to the cart/localStorage");
    // get the item and update it
    var item = localStorage.getItem(itemToGet);
    item = JSON.parse(item);
    item.quantity++;
    localStorage.setItem(itemToGet, JSON.stringify(item));
    // we update the quantity on the screen (DOM operation)
    $(this).parent().parent().prev().children(".cart-quantity").html("Quantity: " + item.quantity);
    // we update the totalPrice on the screen, true stands for add operation
    updateCartTotalPrice(item.price, true);
});


// we put localStorage key together using _this (the position in the DOM)
// and we use "action" can stand for add, remove, bookId
function getLocalStorageKey(_this, action) {
    var data = $(_this).data(action);
    return("itemId"+data);
}

// Controller/Cart/Index
// not logged in user
// remove one item from the cart
$(".grid-cart").on('click', ".cart-remove", function() {

    var cartTotalQuantity = $(".cart-header").data("amount");
    cartTotalQuantity--;
    updateCartTotal(cartTotalQuantity);
    // put the localStorage key together
    var itemToGet = getLocalStorageKey(this, "remove");
    console.log("/Cart/Index - not logged in - removing item from the cart");
    // get the item and update it
    var item = localStorage.getItem(itemToGet);
    item = JSON.parse(item);
    item.quantity--;

    if(item.quantity === 0) {
        // we remove the item from the cart, the DOM and localStorage
        $(this).parent().parent().parent().remove();
        // remove the item from the localStorage 
        localStorage.removeItem(itemToGet); 
    } else {
        // find item in the DOM and update the quantity 
        $(this).parent().parent().prev().children(".cart-quantity").html("Quantity: " + item.quantity);
        // update localStorage
        localStorage.setItem(itemToGet, JSON.stringify(item));
    }
    updateCartTotalPrice(item.price, false);
});

// Controller/Cart/Index
// not logged in user 
// clears one item from the cart
$(".grid-cart").on('click', ".cart-clear-item", function() {

    

    // put the localStorage key together
    var itemToGet = getLocalStorageKey(this, "removeall");
    console.log("/Cart/Index - not logged in - clearing item from the cart");
    
    // get the item from localStorage
    var item = localStorage.getItem(itemToGet);
    item = JSON.parse(item);

    // update the total cart items in the header
    var cartTotalQuantity = $(".cart-header").data("amount");
    cartTotalQuantity = cartTotalQuantity - item.quantity;
    updateCartTotal(cartTotalQuantity);

    // we update the price
    updateCartTotalPrice(item.price * item.quantity, false);
    // we remove the item from the cart (DOM)
    $(this).parent().parent().parent().remove();
    // we remove the item from the localStorage 
    localStorage.removeItem(itemToGet); 
    
});

// Controller/Cart/Index
// not logged in user 
// clears all the items from the cart
$(".cart-item-clear-all").on("click", function(e) {
    console.log('im not logged in - clearing all the items from the cart');
    // update the total price on the screen to be "your cart is empty"
    setTotalPrice(0);
    // we set the total cart items in the header to be 0
    updateCartTotal(0);
    // clear localStorage
    localStorage.clear(); 
    // Remove all cart items from the DOM
    $(".grid-cart").remove();
})
/* -------------------------------- */

/* -------- Home/Index (frontpage) -------- */

// Controller/Home/Index
// logged in user 
// adds item to cart from the front page
// we send the info to Controller/Cart/LoggedInUserCartAction too update the database-table
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

// Controller/Home/Index
// logged in user on the front-page
// adds one book to the cart from the front page
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
    alert("we added one book to the cart");
    // update the total cart items in the header
    var userTotalCartQuantity = $(".user-cart-header").data("amount");
    userTotalCartQuantity++;
    updateUserCartTotal(userTotalCartQuantity);
});

// Controller/Home/Index
// not logged in user 
// adding book from front-page 
$(".add-book").on("click", function(e) {

    var cartTotalQuantity = $(".cart-header").data("amount");
    cartTotalQuantity++;
    updateCartTotal(cartTotalQuantity);

    // we get the localStorage key and then the item
    var storageKey = getLocalStorageKey(this, "book");
    var itemFromStorage = localStorage.getItem(storageKey);
    

    // if we get some item then the item exist, so we just increment quantity by 1
    if(itemFromStorage) {
        console.log("/Controller/Home/Index - Not logged in user - adding book to cart/localStorage");
        item = JSON.parse(itemFromStorage);
        item.quantity++;
        localStorage.setItem(storageKey, JSON.stringify(item));
    } else {
        // here we create new book to add to the localStorage
        console.log("/Controller/Home/Index - Not logged in user - adding new book to cart/localStorage");
        var bookId = $(this).data("book");
        var price = $(this).data("price");
        // get the title
        var title = $(this).prev().prev().prev().text();
        var bookToAdd = {};
        bookToAdd['itemId'] = bookId;
        bookToAdd['quantity'] = 1;
        bookToAdd['price'] = price;
        bookToAdd['title'] = title
        localStorage.setItem(storageKey, JSON.stringify(bookToAdd));
    }
});



// Controller/Home/Details
// all users
// getting all comments
if ($("#user-comments").length > 0)  {
    console.log("--- load comments --- ");
    //var bookId = $(this).find("#review-submit").data("id");
    var bookId = $("#reviewSection").data("isbn");
    console.log("book Id " +  bookId);
    var dataType = 'application/json; charset=utf-8';
    $.ajax({
        type: 'POST',
        url: '/Home/AllReviews',
        dataType: 'json',
        contentType: dataType,
        data: JSON.stringify(bookId),
        success: function(result) {
            console.log('Data received: ');
            console.log(result);

            for(var i = 0; i < result.length; i++) {
                $("#user-comments").append(
                    "<div>" + 
                        "<p> User: " + result[i].customerID + " said: " + result[i].comment +  " rating: " + result[i].rating + "</p>" +
                    "</div>"
                );
            }
            // for-loop í gegnum öll komment
            // og appenda þeim inn
            /*
            $("#user-comments").append(
                "<div>" + 
                    "<p>" + result.user + " said:" + result.comment +  " rating: " + result.rating + "</p>" +
                "</div>"
            );
            */
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log("Cart Post: Status: " + textStatus + " Error: " + errorThrown);
        } 
    });
}

// Controller/Home/Details
// logged in user 
// adding rating and comment on book
$("#book-detail-review-form").on("submit", function(e) {
    e.preventDefault(); // we prevent browser re-load
    console.log("/Controller/Home/Details - logged in user - submit rating and comment");
    console.log($(this).find("#review-submit").data("id"));
    var bookId = $(this).find("#review-submit").data("id");
    //console.log($("#user-comment").val());
    //console.log($("#"))
    //console.log(e);
    
    var dataArray = $("#book-detail-review-form").serializeArray();
    // parseInt(text, 10);
    var rating = parseInt(dataArray[0].value, 10);
    //console.log(dataArray);
    item = { 
        id: bookId, 
        rating: rating, 
        comment: dataArray[1].value
    }

    console.log(item);

    // þetta er óþarfi
    var dataType = 'application/json; charset=utf-8';
    $.ajax({
        type: 'POST',
        url: '/Home/AddComment',
        dataType: 'json',
        contentType: dataType,
        data: JSON.stringify(item),
        success: function(result) {
            console.log('Data received: ');
            console.log(result);
            $("#user-comments").append(
                "<div>" + 
                    "<p>" + result.user + " said:" + result.comment +  " rating: " + result.rating + "</p>" +
                "</div>"
            );
            /*
            if(result === false) {
                window.location.href = "http://localhost:5000/Account/Login";
            } else {
                window.location.href = "http://localhost:5000/Checkout";
            } 
            */ 
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log("Cart Post: Status: " + textStatus + " Error: " + errorThrown);
        } 
    });
    /*
    var failed = false;
    $.post("Home/AddComment", dataArray, function(data, status) {
        console.log(status);
    }).fail(function(err) {
        console.log(err);
        failed = true;
    });
    if(failed === false) {
        var markup = "<div> success </div>";
        console.log(markup);
    }
    */
});

/*
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
                /*
                $(".grid-cart").append(
                    "<div class=grid-cart-item>" +
                        "<div class=grid-cart-item-title>" + result[j].title + "</div>" +
                        "<div class=grid-cart-item-price>" +
                            "<div class=cart-quantity>" + "Quantity: " + result[j].quantity + "</div>" +
                            "<div>" + "Price for each: " + result[j].price + "</div>" +
                        "</div>" +
                        "<div class=grid-cart-buttons>" +
                            "<div>" + "<button " + addItemData + " class=\"cart-add btn btn-success\"> + </button>" + "</div>" +
                            "<div>" + "<button " + removeItemData + " class=\"cart-remove btn btn-danger\"> - </button>" + "</div>" +
                        "</div>" +
                    "</div>");
                
            }     
            
            setTotalPrice(totalPrice)
            //getTotalPrice(totalPrice);
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            $("#cart #cart-loader").toggleClass("loader");
            $("#cart-items").html("<h4> Something went wrong. </h4>");
            console.log("Cart Post: Status: " + textStatus + " Error: " + errorThrown);
        } 
    });
    */

        //var allItemsInCart = [];
    /*
    for (var i = 0; i < localStorage.length; i++) {
        itemStoreId = localStorage.getItem(localStorage.key(i));
        key = localStorage.key(i);
        if(key.startsWith('itemId')) {
            if(itemStoreId !== 'INFO' && itemStoreId !== null && itemStoreId !== undefined) {
                singleItem = JSON.parse(itemStoreId);
                var item = { 
                    itemId: singleItem.itemId, 
                    quantity: singleItem.quantity, 
                    price: singleItem.price,
                    title: singleItem.title
                };
                totalPrice = totalPrice + singleItem.price * singleItem.quantity;
                allItemsInCart.push(item);
                addItemData = "data-add="+ singleItem.itemId;
                removeItemData = "data-remove="+ singleItem.itemId;
                clearItemData = "data-removeall="+ singleItem.itemId;
                $(".grid-cart").append(
                    "<div class=grid-cart-item>" +
                        "<div class=grid-cart-item-title>" + singleItem.title + "</div>" +
                        "<div class=grid-cart-item-price>" +
                            "<div class=cart-quantity>" + "Quantity: " + singleItem.quantity + "</div>" +
                            "<div>" + "Price for each: " + singleItem.price + "</div>" +
                        "</div>" +
                        "<div class=grid-cart-buttons>" +
                            "<div>" + 
                                "<button " + addItemData + " class=\"cart-add btn btn-default fas fa-plus\"></button>" + 
                            "</div>" +
                            "<div>" + 
                                "<button " + removeItemData + " class=\"cart-remove btn btn-default fas fa-minus\"></button>" + 
                            "</div>" +
                            "<div>" + 
                                "<button " + clearItemData + " class=\"cart-clear-item btn btn-default fa fa-trash\"></button>" + 
                            "</div>" +
                        "</div>" +
                    "</div>");
            }
        }
    }
    */

    /*
        addItemData = "data-add="+ allItems[j].itemId;
        removeItemData = "data-remove="+ allItems[j].itemId;
        clearItemData = "data-removeall="+ allItems[j].itemId;
        console.log(addItemData);
        
        $(".grid-cart").append(
            "<div class=grid-cart-item>" +
                "<div class=grid-cart-item-title>" + allItems[j].title + "</div>" +
                "<div class=grid-cart-item-price>" +
                    "<div class=cart-quantity>" + "Quantity: " + allItems[j].quantity + "</div>" +
                    "<div>" + "Price for each: " + allItems[j].price + "</div>" +
                "</div>" +
                "<div class=grid-cart-buttons>" +
                    "<div>" + 
                        "<button " + addItemData + " class=\"cart-add btn btn-default fas fa-plus\"></button>" + 
                    "</div>" +
                    "<div>" + 
                        "<button " + removeItemData + " class=\"cart-remove btn btn-default fas fa-minus\"></button>" + 
                    "</div>" +
                    "<div>" + 
                        "<button " + clearItemData + " class=\"cart-clear-item btn btn-default fa fa-trash\"></button>" + 
                    "</div>" +
                "</div>" +
            "</div>");
        */

/*      
        
        // þetta er óþarfi
        //var dataType = 'application/json; charset=utf-8';
        $.ajax({
            type: 'POST',
            url: '/Cart/Buy',
            dataType: 'json',
            contentType: dataType,
            //data: JSON.stringify(allItemsInCart),
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
        */

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
