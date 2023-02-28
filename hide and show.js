$("#datecheckbox").change(function () {
            var datecheckStatus = $("#datecheckbox").is(":checked") == true ? 1 : 0;
            if (datecheckStatus == 1) {
                $("#startendaction").show();
            }
            else {
                $("#startendaction").hide();
            }
            
        });