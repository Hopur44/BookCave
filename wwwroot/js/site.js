// Write your JavaScript code.
console.log("javascript code .js");

// All Controllers
// All users
// We update cart total price
if ($(".cart-header").length > 0)  {
    
    var total = 0;
    var items = getAllItemsFromLocalStorage();
    for(var i = 0; i < items.length; i++) {
        total = total + items[i].quantity;
    }
    updateCartTotal(total)
}

// Logged in user
// We get total cart quantity
if ($(".user-cart-header").length > 0)  {
    console.log("user cart header");    
    getTotalUserCartQuantity();
}

function getTotalUserCartQuantity() {
    $.ajax({
        type: 'GET',
        url: '/Cart/GetTotalQuantity',
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
    
    var amount = $(".user-cart-header").data("amount");
    sendActionToCartController(item);

    // we update the total cart items in the header
    var userTotalCartQuantity = $(".user-cart-header").data("amount");
    userTotalCartQuantity++;
    updateUserCartTotal(userTotalCartQuantity); 
    
    // we update the item quantity on the screen 
    updateQuantityHtml(this, quantity+1);
    // we update the total price on the screen
    updateCartTotalPrice(item.price, true); // true is add price to totalPrice

});

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
    var quantity = getUserQuantity(this);
    var price = getUserPrice(this); 
    var bookId = getUserBookId(this, "useritemremove");
    console.log("Logged in user removing: " + bookId + " and price: " + price + " and quantity: " + quantity);
    var item = createItemForAjax(bookId, quantity, price, false);
    
    // we send the action to the controller to remove one item
    sendActionToCartController(item);
    
    // we update the total cart items in the header
    var userTotalCartQuantity = $(".user-cart-header").data("amount");
    userTotalCartQuantity--;
    updateUserCartTotal(userTotalCartQuantity);

    // we update in totalPrice on the screen 
    updateCartTotalPrice(item.price, false); // false is deduct price from totalPrice
    // if quantity is going to be zero then we remove this row from the DOM
    if(quantity - 1 === 0 ) {
        $(this).parent().parent().parent().remove();
    } else {
        updateQuantityHtml(this, quantity-1);  
    } 
});

// Controller/Cart/Index
// logged in user 
// clears the cart of book with this Id
$(".user-cart-item-clear").on("click", function(e) {

    var quantity = getUserQuantity(this); 
    var price = getUserPrice(this);
    var bookId = getUserBookId(this, "useritemclear");
    var item = createItemForAjax(bookId, 1, price, false);
    console.log("Logged in user clearing: " + bookId + " from his cart");
    
    // we send the action to the controller to remove one item
    sendActionToCartController(item);
    
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

    // we tell the controller to clear the cart
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

    $(".close-cart-modal").on("click", function(e) {
        window.location.href = "http://localhost:5000/Account/Login";
    });
}

// Controller/Cart/Index
// logged in user 
// This user cart is empty
if ($("#logged-in-empty-cart").length > 0)  {
    
    // we get all items from localStorage
    var items = getAllItemsFromLocalStorage();
    if(items.length > 0) {

        $("#logged-in-empty-cart").append("<div class=\"user-empty-cart-container\"></div>");
        $(".user-empty-cart-container").append("<h4>We found these items that were added to cart on this device when you were not logged in. </h4>");
        
        for(var j = 0; j < items.length; j++) {
            $(".user-empty-cart-container").append(
                "<p>" + (j + 1) + ". " + items[j].title + ". Quantity: " + items[j].quantity + "</p>");
        }
        
        $(".user-empty-cart-container").append("<strong> Do you want to add them too your cart? </strong>");
        $(".user-empty-cart-container").append("<span class=\"user-storage-add-yes btn btn-default\"> yes </span>");
        $(".user-empty-cart-container").append("<span class=\"user-storage-add-no\ btn btn-default\"> no </span>");
    }
}

$('.user-empty-cart-container').on('click', '.user-storage-add-no', function() {
    console.log("this user did not want to add items from localStorage to his cart");
    ($(".user-empty-cart-container")).fadeOut();
    localStorage.clear();
});

// Controller/Cart/Index
// logged in user 
// This users wants to add items from localStorage to his cart
$('.user-empty-cart-container').on('click', '.user-storage-add-yes', function() {
    console.log("user wants to add items from localS to database");

    var dataType = 'application/json; charset=utf-8';
    var items = getAllItemsFromLocalStorage();
    $.ajax({
        type: 'POST',
        url: '/Cart/AddAllCartItems',
        dataType: 'json',
        contentType: dataType,
        data: JSON.stringify(items),
        success: function(result) {
            console.log('Data received: ');
            console.log(result);
            window.location.href = "http://localhost:5000/Cart";
            localStorage.clear();
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log("Cart/LoggedInUserCartAction Post: Status: " + textStatus + " Error: " + errorThrown);
        } 
    });
        
});

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
    //alert("we added one book to the cart");
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
        var title = $(this).data("title");
        // get the title
        //var title = $(this).prev().prev().prev().text();
        var bookToAdd = {};
        bookToAdd['itemId'] = bookId;
        bookToAdd['quantity'] = 1;
        bookToAdd['price'] = price;
        bookToAdd['title'] = title
        localStorage.setItem(storageKey, JSON.stringify(bookToAdd));
    }
});

//Controller/Home/Index
//Filters books to only show one genre by using it as a search string
$("#genre-filter a").click(function() {
   var SearchString = $(this).data('genre');
   console.log(SearchString)
   window.location.href = "/Home/Index?SearchString=" + SearchString;
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
                    "<div class=comments>" +
                        "<p>"+ "<strong> " + result[i].customerName + "</strong>. Rating: " + result[i].rating +  " stars.</p>" +
                        "<p>  " + result[i].comment +  ".</p>" + 
                    "</div>"
                );
            }
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
    
    // we create the object with bookId, comment and rating to send to the controller
    var dataArray = $("#book-detail-review-form").serializeArray();
    console.log(dataArray)
    if(dataArray[0].value === "" || dataArray[1].value === "") {
        alert("Please comment and rate the book")
    } else {
        var rating = parseInt(dataArray[0].value, 10);
        item = { 
            id: bookId, 
            rating: rating, 
            comment: dataArray[1].value
        }

        var dataType = 'application/json; charset=utf-8';
        $.ajax({
            type: 'POST',
            url: '/Home/AddComment',
            dataType: 'json',
            contentType: dataType,
            data: JSON.stringify(item),
            success: function(result) {

                if(result.firstComment === false) {
                    console.log('Data received: ');
                    console.log(result);
                    var name = result.user.split("@");
                    $("#user-comments").append(
                        "<div class=comments>" +
                                "<p>"+ "<strong> " + name[0] + "</strong>. Rating: " + result.rating +  " stars.</p>" +
                                "<p>  " + result.comment +  ".</p>" + 
                            "</div>"
                    );
                } else {
                    alert("Thank you but you already made a comment about this book.");
                }
                
            },
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                console.log("Cart Post: Status: " + textStatus + " Error: " + errorThrown);
            } 
        });    
    }
    
});

// All users
// We sort the books by price
// https://stackoverflow.com/questions/41439283/sorting-dom-elements-using-pure-js
$("#order-by-price").on("click", function(e) { 
    console.log("found order by name button");
    var parent = document.querySelector('.grid-container');
    [].slice.call(parent.children)

    .sort(function(a, b) {
        // get text content in .price and return difference
        return getPrice(a) - getPrice(b);
        // iterate and append again in new sorted order
      }).forEach(function(ele) {
        parent.appendChild(ele);
      })    
});

// All users
// We sort the books by rating
// https://stackoverflow.com/questions/41439283/sorting-dom-elements-using-pure-js
$("#order-by-rating").on("click", function(e) { 
    console.log("found order by rating button");
    console.log($(".grid-container"));
    var parent = document.querySelector('.grid-container');
    [].slice.call(parent.children)

    .sort(function(a, b) {
        // get text content in .price and return difference
        return getRating(b) - getRating(a);
        // iterate and append again in new sorted order
      }).forEach(function(ele) {
        parent.appendChild(ele);
      })    
});

// All users
// We sort the books by title
// https://stackoverflow.com/questions/41439283/sorting-dom-elements-using-pure-js
$("#order-by-title").on("click", function(e) { 
    console.log("found order by name button");
    console.log($(".grid-container"));
    var parent = document.querySelector('.grid-container');
    [].slice.call(parent.children)

    .sort(function(a, b) {
        // get text content in .price and return difference
        var a = getName(a);
        var b = getName(b);
        if (a < b) {
            return -1;
        }
        else if ( a > b) {
            return 1;
        } else {
            return 0;
        }
        // iterate and append again in new sorted order
      }).forEach(function(ele) {
        parent.appendChild(ele);
      })    
});

function getPrice(ele) {
    var _this = ele.children[4];
    var price = $(_this).data("price");
    return price;
}

function getRating(ele) {
    var _this = ele.children[2];
    var rating = $(_this).data("rating");    
    return rating;
}

function getName(ele) {
    var _this = ele.children[1];
    var title = $(_this).data("title");
    return title;
}