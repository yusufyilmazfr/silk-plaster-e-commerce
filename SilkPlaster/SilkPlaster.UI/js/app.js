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

function GetMyGetWishListPage(callback) {
    new Get('/WishList/MyWishList', callback);
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
    ShowSweetAlert('error', 'Giriş Yapın.', 'Bu işlemi gerçekleştirmeden önce lütfen giriş yapınız..', `<a href='/account/login'><b>Giriş Yap</b></a>`);
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