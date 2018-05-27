var doanhthuController = {
    init: function() {
        doanhthuController.registerEvent();
        doanhthuController.loadData();
    },
    registerEvent: function() {

    },
    loadData:function() {
        $.ajax({
            url: '/Admin/LoadData',
            type: 'GET',
            dataType: 'json',
            success: function(response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data,
                        function(i, item) {
                            html += Mustache.render(template,
                                {
                                    MAHD: item.MAHD,
                                    NGAYGIAO: item.NGAYGIAO,
                                    HOTEN: item.KHACHANG.HOTEN,
                                    TongTien: item.TongTien
                                });
                        });
                    
                    $('#dataDoanhThu').html(html);
                }
            }
        });
    }
}
doanhthuController.init();