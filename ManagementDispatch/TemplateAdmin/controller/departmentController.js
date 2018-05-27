var department = {
    init: function () {
        department.registerEvents();
    },
    registerEvents: function () {
        $('.abc').off('click').on('click', function (e) {

            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/Department/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status) {
                        btn.text("Đang sử dụng");
                    } else {
                        btn.text("Đã khóa");
                    }


                }
            });
        });
    }
}
department.init();