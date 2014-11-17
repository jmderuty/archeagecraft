function ItemsViewModel(app, dataModel) {
    var self = this;

    self.id = ko.observable(0);
    self.newItemName = ko.observable('');
    self.newItemMerchantCost = ko.observable(0);
    self.newItemVocationBadgesCost = ko.observable(0);
    self.newItemCategory = ko.observable('');

    self.template = "items";
    self.items = ko.observableArray();
    self.categories = ko.observableArray();
    self.cat = ko.observable('');
    self.refresh = function () {
        $.ajax({
            method: 'get',
            url: '/api/items?cat=' + self.cat(),
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
            },
            success: function (data) {
                self.items.removeAll();
                for (i = 0; i < data.length; i++) {
                    self.items.push(data[i]);
                }

            }
        });
    };
    
    self.refreshCategories = function () {
        $.ajax({
            method: 'get',
            url: '/api/items/categories',
            contentType: "application/json; charset=utf-8",
            headers: {
            },
            success: function (data) {
                self.categories.removeAll();
                for (i = 0; i < data.length; i++) {
                    if (data[i] == null) {
                        data[i] = '';
                    }
                    self.categories.push(data[i]);
                }
                if (self.cat() == '') {
                    self.cat(data[0]);
                }
                self.refresh();
            }
        });
    };
    self.create = function () {
        if (self.newItemName() != '') {
            $.ajax({
                method: 'post',
                url: '/api/items',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(
                    {
                        name: self.newItemName(),
                        merchantCost: self.newItemMerchantCost(),
                        vocationBadgeCost: self.newItemVocationBadgesCost(),
                        category: self.newItemCategory()
                    }),
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                success: function () {
                    self.refresh();
                }
            });
        }
    };

    Sammy(function () {
        this.get('#items', function (context) {
            self.cat('');
            app.viewModel(self);
            self.refreshCategories();
        });
        this.get('#items/:cat', function (context) {
            self.cat(context.params['cat']);
            app.viewModel(self);
            self.refreshCategories();
        });
        //this.get('/', function () { this.app.runRoute('get', '#home') });
    });

    return self;
}

app.addViewModel({
    name: "Items",
    bindingMemberName: "items",
    factory: ItemsViewModel
});
