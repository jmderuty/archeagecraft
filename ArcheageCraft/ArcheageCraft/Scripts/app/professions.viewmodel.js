function ProfessionsViewModel(app, dataModel) {
    var self = this;

    self.professions = ko.observableArray();
    self.template = "professions";
    self.name = ko.observable('');
    self.deleteProfession = function (profession) {
        if (confirm("Êtes-vous sûr?")) {
            $.ajax({
                method: 'delete',
                url: '/api/professions/' + profession.professionId,
                contentType: "application/json; charset=utf-8",
                headers: {
                    'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
                },
                success: function (data) {
                    self.refresh();
                }
            });
        }
    };

    self.create = function () {
        $.ajax({
            method: 'post',
            url: '/api/professions',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({Name: self.name()}),
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
            url: '/api/professions',
            contentType: "application/json; charset=utf-8",
            headers: {
                'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
            },
            success: function (data) {
                self.professions.removeAll();
                for (i = 0; i < data.length; i++) {
                    self.professions.push(data[i]);
                }
                
            }
        });
    };

    Sammy(function () {
        this.get('#professions', function () {
            // Make a call to the protected Web API by passing in a Bearer Authorization Header
            
            app.viewModel(self);
            self.refresh();
        });
        //this.get('/', function () { this.app.runRoute('get', '#home') });
    });

    return self;
}

app.addViewModel({
    name: "Professions",
    bindingMemberName: "professions",
    factory: ProfessionsViewModel
});
