var clickId = "";
var chat = $.connection.chatHub;
chat.client.sendMessage = function (magId, allData) {
    try {
        var filteredUserNotiNo = allData.filter((noti) => {
            return noti.ExecutiveId == userId;
        });
        var NotiNo = filteredUserNotiNo[0]["NotiNo"];
        if (NotiNo > 0) {
            $("#empNotitop").html(NotiNo);
        }
    } catch (e) {

    }
};
$.connection.hub.start().done(function () {
    //$(document).on("click", "#btnApproved,#btnSaveRequisition,#btnSaveUpdateRequisition,#btnSaveProductIssue", function () {
    //    clickId = $(this).attr("id");
    //});
    $(document).on("click", "#btnSaveNoti", function () {
        clickId = $(this).attr("id");
    });
    $(document).ajaxSuccess(function () {
        //var arr = ['btnApproved', 'btnSaveRequisition', 'btnSaveUpdateRequisition', 'btnSaveProductIssue'];
        var arr = ['btnSaveNoti'];
        if (arr.find(i => i == clickId)) {
            clickId = "";
            $.ajax({
                type: "POST",
                url: url.getNotification,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ isAllExecutive: true }),
                dataType: "json",
                success: function (data) {
                    if (data.Status == true) {
                        chat.server.send(magId = 0, allData = data.data);
                        allData = data.data;
                    } else {
                        $.alert.open("error", data.Message);
                    }
                },
            });
        };
    });
});