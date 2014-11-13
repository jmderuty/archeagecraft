function ItemViewModel(app, dataModel) {
    var self = this;

    self.crafts = ko.observableArray();
    self.template = "item";
    self.name = ko.observable('');
    self.vocationBadgeCost = ko.observable(0);
    self.merchantCost = ko.observable(0);
    self.id = ko.observable(0);
    self.professions = ko.observableArray();

    self.remove = function () {
        if (confirm("Êtes-vous sûr?")) {
            $.ajax({
                method: 'delete',
                url: '/api/items/' + self.id(),
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                success: function (data) {
                    app.navigateToItems();
                }
            });
        }
    };

    self.newRecipe = function () { };
   
    self.update = function () {
        $.ajax({
            method: 'put',
            url: '/api/items/' + self.id(),
            contentType: "application/json; charset=utf-8",
            data:JSON.stringify(
                {
                    vocationBadgeCost: self.vocationBadgeCost(),
                    merchantCost: self.merchantCost(),
                    name: self.name(),
                    itemId:self.id()
                }),
            headers: {
                'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
            },
            success: function (data) {
                self.refresh();
            }
        });
    };
    self.refresh = function () {
        $.ajax({
            method: 'get',
            url: '/api/items/' + self.id(),
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
            },
            success: function (data) {
                self.name(data.name);
                self.vocationBadgeCost(data.vocationBadgeCost);
                self.merchantCost(data.merchantCost);
                self.refreshCraft();
            }
        });
    };

    self.loadProfessions = function () {
        $.ajax({
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    self.professions.push(data[i]);
                }
            }
        });
    }

    self.refreshCraft = function () {
        $.ajax({
            method: 'get',
            url: '/api/items/recipes/' + self.id(),
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
            },
            success: function (data) {
                self.crafts.removeAll();
                for (var i = 0; i < data.length; i++) {
                    self.crafts.push(new RecipeViewModel(app, self, data[i]));
                }

            }
        });
    }


    Sammy(function () {

        this.get('#items/:id', function (context) {
            var id = context.params['id']
            
            self.id(id);
            app.viewModel(self);
            self.refresh();
            self.loadProfessions();
        });

    });

    return self;
}

function RecipeViewModel(app, itemViewModel, craft) {
    self = this;

    self.laborCost = ko.observable(craft.laborCost);
    self.production = ko.observable(craft.production);
    self.ingredients = ko.observableArray();

    for (var i = 0; i < craft.ingredients.length; i++) {
        self.ingredients.push(craft.ingredients[i]);
    }

    self.profession = ko.observable();
    var professions = itemViewModel.professions;
    if (data.professionId != null) {
        for (var i = 0; i < professions.length; i++) {
            if (professions[i].professionId == data.professionId) {
                ko.observable(professions[i]);
                break;
            }
        }
    }
    self.items = ko.observableArray();

    self.newIngredient = ko.observable();
    self.newIngredientCount = ko.observable(0);

    self.removeIngredient = function (ingredient) {

        $.ajax({
            method: 'delete',
            url: '/api/ingredients/' + ingredient.id,
            success: function () {
                self.refreshIngredients();
            }
        });
    };
    self.refreshIngredients = function () {
        $.ajax({
            method: 'get',
            url: 'api/crafts/ingredients/' + self.id(),
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
            },
            success: function (data) {
                self.ingredients.removeAll();
                for (var i = 0; i < data.length; i++) {
                    self.ingredients.push(data[i]);
                }
            }
        });
    }
    self.addIngredient = function (ingredient) {
        if (self.NewIngredient() == null || self.NewIngredientCount() == 0) {
            return;
        }

        $.ajax({
            method: 'post',
            url: 'api/ingredients/',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                itemId: self.newIngredient().itemId,
                count: self.newIngredientCount(),
                craftId: craft.id
            }),
            headers: {
                'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
            },
            success: function () {
                self.NewIngredient(null);
                self.NewIngredientCount(0);
                self.refreshIngredients();
            }
        });
    };

    $.ajax({
        method: 'get',
        url: 'api/items',
        contentType: "application/json; charset=utf-8",
        headers: {
            'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
        },
        success: function (data) {
            self.items.removeAll();
            for (var i = 0; i < data.length; i++) {
                if (data[i].itemId != itemViewModel.id) {
                    self.items.push(data[i]);
                }
            }
        }
    });
}
app.addViewModel({
    name: "Item",
    bindingMemberName: "item",
    factory: ItemViewModel
});
