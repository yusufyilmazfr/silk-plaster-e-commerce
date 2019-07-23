function DecreaseProductCount(productId) {
    new Post('/Basket/DecreaseProductCount', { productId: productId }, function (data) {
        data = JSON.parse(data);

        if (data.result) {

            GetMyGetBasketPage(function (value) {
                document.getElementById('myBasketPage').innerHTML = value;
            });

            FillQuicklyBasket();
        }
        else {
            ShowSweetAlert('error', 'Ooopss!', data.message);
        }
    });
}

function IncreaseProductCountInBasketItem(productId) {
    new Post('/Basket/IncreaseProductCount', { productId: productId }, function (data) {
        data = JSON.parse(data);

        if (data.result) {

            GetMyGetBasketPage(function (value) {
                document.getElementById('myBasketPage').innerHTML = value;
            });

            FillQuicklyBasket();
        }
        else {
            ShowSweetAlert('error', 'Ooopss!', data.message);
        }
    });
}

function RemoveProductInBasket(productId) {
    new Post('/Basket/Remove', { productId: productId }, function (data) {
        data = JSON.parse(data);

        if (data.result) {

            ShowSweetAlert('success', 'Silme İşlemi Başarılı!', 'Ürün başarıyla silindi!');

            GetMyGetBasketPage(function (value) {
                document.getElementById('myBasketPage').innerHTML = value;
            });

            FillQuicklyBasket();
        }
        else {
            ShowSweetAlert('error', 'Oops..', data.message.ErrorMessage);
        }
    });
}

function AddWishList(productId) {
    new Post('/WishList/Add', { productId: productId }, function (status) {
        let result = JSON.parse(status).result;

        if (result) {
            ShowSweetAlert('success', 'Başarılı!', 'Ürün beğendikleriniz arasına eklendi:)');

            FillQuicklyWishList();
        }
        else {
            ShowSweetAlert('error', 'Oops..', 'Sunucu ile bağlantı kurulamadı, lütfen daha sonra tekrar deneyiniz.!');
        }
    });
}

function AddProductInBasket(productId, productCount) {
    new Post('/Basket/AddProductInBasket', { productId: productId, productCount: productCount }, function (data) {

        data = JSON.parse(data);

        if (data.result) {
            ShowSweetAlert('success', 'Başarılı!', 'Ürün sepetinize eklendi:)');

            FillQuicklyBasket();
        }
        else {
            ShowSweetAlert('error', 'Oops..', data.message.ErrorMessage);
        }

    });
}

function RemoveProductInWishList(productId) {
    new Post('/WishList/Remove', { productId: productId }, function (data) {
        data = JSON.parse(data);

        if (data.result) {
            ShowSweetAlert('success', 'Silme İşlemi Başarılı!', 'Ürün başarıyla silindi!');

            GetMyGetWishListPage(function (value) {
                document.getElementById('myWishListPage').innerHTML = value;
            });

            FillQuicklyWishList();
        }
        else {
            ShowSweetAlert('error', 'Oops..', data.message.ErrorMessage);
        }
    });
}

function FillQuicklyWishList() {
    GetWishList(function (data) {
        document.getElementById('myWishList').innerHTML = data;
    });

    GetMyWishListCount(function (data) {
        document.getElementById('myWishListCount').innerHTML = data;
    });
}

function FillQuicklyBasket() {
    GetBasket(function (data) {
        document.getElementById('myBasket').innerHTML = data;
    });

    GetBasketItemCount(function (data) {
        document.getElementById('basketItemCount').innerHTML = data;
    });
}

function GetMyGetWishListPage(callback) {
    new Get('/WishList/MyWishList', callback);
}

function GetMyGetBasketPage(callback) {
    new Get('/Basket/MyBasket', callback);
}

function ShowSweetAlert(type, title, text, footer) {
    Swal.fire({
        title,
        text,
        type,
        footer
        }
    );
        }

function ShowLoginSweetAlert() {
    ShowSweetAlert('error', 'Giriş Yapın.', 'Bu işlemi gerçekleştirmeden önce lütfen giriş yapınız..', `<a href='/hesabim/giris-yap'><b>Giriş Yap</b></a>`);
}

function GetWishList(callback) {
    new Get('/Basket/WiewQuicklyWishList', callback);
}

function GetMyWishListCount(callback) {
    new Get('/WishList/GetMyWishListCount', callback);
}

function GetBasket(callback) {
    new Get('/Basket/WiewQuicklyBasket', callback);
}

function GetBasketItemCount(callback) {
    new Get('/Basket/GetBasketItemCount', callback);
}

function QuicklyViewProduct(productId) {
    let relativePath = '/Product/QuicklyViewProduct/' + productId;

    new Get(relativePath, function (value) {
        document.getElementById('popup1').innerHTML = value;
    });
}