function ItemsViewModel(app, dataModel) {
    var self = this;

    self.id = ko.observable(0);
    self.newItemName = ko.observable('');
    self.newItemMerchantCost = ko.observable(0);
    self.newItemVocationBadgesCost = ko.observable(0);

    self.template = "items";
    self.items = ko.observableArray();

    self.refresh = function () {
        $.ajax({
            method: 'get',
            url: '/api/items',
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
                        vocationBadgeCost : self.newItemVocationBadgesCost()
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
            
            app.viewModel(self);
            self.refresh();
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
