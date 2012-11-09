var app = $.sammy('#main_content', function () {

    this.use(Sammy.Mustache, 'html').use(Sammy.Title);

    this.post('#/order/:id', function (context) {

        var fields = {
            email: context.params['email'],
            address: context.params['address'],
            quantity: parseInt(context.params['quantity'])
        };

        $.post('/app/order/' + context.params['id'],
                fields,
                function (response) {

                    if (response.success) {
                        context.title('"Order Succeeded!" - Sammy Fourth Coffee');
                        context.partial(viewPath('success'));
                    }
                }, 'json');
    });

    this.get('#/order/:id', function (context) {

        this.load('/app/product/' + context.params['id'])
            .then(function (response) {

                var title = 'Place Your Order : ' + response.Name;

                context.title(title + ' - Sammy Fourth Coffee');

                var order = {
                    productId: response.Id,
                    productName: response.Name,
                    productPicture: response.Picture,
                    productPrice: response.Price,
                    quantity: 1,
                    amount: function () { return this.productPrice * this.quantity; }
                };

                context.partial(viewPath('order'), { title: title, order: order });
            });
    });

    this.get('#/about', function (context) {

        this.title('About Us - Sammy Fourth Coffee');

        this.partial(viewPath('about'));
    });

    this.get('#/', function (context) {

        this.title('Home - Sammy Fourth Coffee');
        this.load('/app/products')
            .then(function (response) {
                context.partial(viewPath('index'), response);
            });
    });

    function viewPath(name) {
        return '/Views/PartnerInfo/' + name + '.html';
    }
});