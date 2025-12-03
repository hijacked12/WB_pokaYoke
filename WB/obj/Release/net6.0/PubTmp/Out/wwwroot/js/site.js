valueCount = 0;
check = 0;
data2 = "false";
newCount = 0;
changedId = "";
manual_color = "";
let ten_num = [];
marked = [];
const count = 1;
id_slider = "";
let real_ten = '';
present = true;
slider_name = "";
slider_name2 = "";
const addr = "http://10.174.24.151:8082/Category/";
//const addr = "http://localhost:5152/Category/";
let line_no = document.getElementById('line_general');
var model = new Object();
var frontend_data = new Object();
const currentPath = window.location.pathname;
var selectionMade = localStorage.getItem("selectionMade");
const sensorDictionary = {
    1: '护套物料盘感应器',
    2: '刷片胶条物料盘感应器',
    3: '纸质套管无聊盘感应器',
    4: '1#组件n9+n8物料盘感应器',
    5: '2#组件oe2+ne3物料盘感应器',
    6: '3#组件物料盘感应器',
    7: '说明书物料盘感应器',
};
modalOverlay = document.querySelector(".overlay");
modal_bod = document.getElementById("modal_body");
setInterval('stoppedTyping()', 5000);
setInterval('clear()', 30000);
setInterval('refresh()', 2000);
setInterval('getPLCdata()', 1500);

if (selectionMade !== null) {
    modal_bod.style.display = "none";
    modalOverlay.style.display = "none";
}

document.addEventListener("DOMContentLoaded", function () {
    var selectLineButton = document.getElementById("selectLine");
    var lineDropdown = document.getElementById("lineDropdown");

    // Check if the user has already made a selection
    var selectionMade = localStorage.getItem("selectionMade");

    if (selectionMade !== null) {
        // Open the modal when the page loads
        if (!currentPath.endsWith("Category/SelectedPN")) {
            modalOverlay.style.display = "none";
            modal_bod.style.display = "none";
        }

        line_no = selectionMade;
        model.X6 = selectionMade;

        $.ajax({
            type: "POST",
            url: addr + "Line_No",
            data: model,
            success: function (data) {
                // Handle the response as needed
            }
        });

        $.ajax({
            type: "POST",
            url: addr + "Index",
            data: model,
            success: function (data) {
            }
        });
    }

        selectLineButton.addEventListener("click", function () {
            var selectedLine = lineDropdown.value;
            line_no = selectedLine;
            model.X6 = selectedLine;

            $.ajax({
                type: "POST",
                url: addr + "Line_No",
                data: model,
                success: function (data) {
                }
            });

            $.ajax({
                type: "POST",
                url: addr + "Index",
                data: model,
                success: function (data) {
                }
            });

            localStorage.setItem("selectionMade", selectedLine);
            modal_bod.style.display = "none";
            modalOverlay.style.display = "none";
        });
    
});

function getPLCdata() {

    if (currentPath.endsWith("Category/SelectedPN")) {
        $.ajax({
            url: addr + "ReceivePlcData",
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                // process the data
                //console.log(parseInt(data));
                //console.log(sensorDictionary[parseInt(data)])
                if (data == 0) {
                    modal_bod.style.display = "none";
                    modalOverlay.style.display = "none";
                }
                else if (data !== 0) {
                    modalOverlay.style.display = "flex";
                    document.getElementById('reason_Number').innerHTML = sensorDictionary[parseInt(data)];
                    modal_bod.style.display = "flex";
                }
            },
            error: (err) => console.error(`Error: ${err}`)
        });

        var popup_empty = document.getElementById("close_emptybox_popup");
        popup_empty.addEventListener("click", function () {
            modal_bod.style.display = "none";
            modalOverlay.style.display = "none";
        });
    }
}


var value1 = document.getElementById('addNum');
function stoppedTyping() {

    // Check if the path ends with "Category/SelectedPN"
    if (currentPath.endsWith("Category/SelectedPN")) {

        if (value1.value.length > 0) {
            document.getElementById('AddBtn').disabled = false;
        } else {
            document.getElementById('AddBtn').disabled = true;
        }
    }
}


//$(document).ready(function () {
//    $('#AddBtn').click(function (e) {
//        $(this.form).ajaxSubmit({
//            target: false,
//            success: function () {
//                //alert("success");
//                check = parseInt(document.getElementById("curr_val").innerHTML);
//                manual_color = document.getElementById('addNum').value;
//                ten_num.push(real_ten);
//                sleepingFunc();
//                //refresh();
//                //colorChange();

//            }
//        });
//    });

//});

//function AddManual() {
//    //alert(clicked_id);
//    //changedId = clicked_id;
//    manual_Input = document.getElementById('addNum').value;
//    var model = new Object();
//    model.X3 = manual_Input

//    $.ajax({
//        type: "POST",
//        url: addr + "AddManual",
//        dataType: "json",

//        data: { data1: model },
//        success: function (data) {
//            alert(data);
//        },
//        fail: function (errMsg) {
//            alert(errMsg);
//        }
//    });
//    alert(manual_Input);
//    $.ajax({
//        url: addr + "AddManual",
//        type: 'GET',
//        dataType: 'json',
//        success: function (data) {
//            // process the data coming back
//            document.getElementById('topUpdate').innerHTML = data;
//            alert(data);

//        },
//    });
//    //refresh();

//}

function sleep(ms) {
    return new Promise(resolveFunc => setTimeout(resolveFunc, ms));
}

function synchronousFunc() {
    for (let i = 0; i < 5; ++i) {
        console.log(i + " - from sync");
    }
}

async function sleepingFunc() {
    for (let i = 0; i < 2; ++i) {
        console.log(i + " - from sleep");
        await sleep(1000);
    }
}

async function refresh() {
    // Get the current URL path
    //const currentPath = window.location.pathname;

    // Check if the path ends with "Category/SelectedPN"
    if (currentPath.endsWith("Category/SelectedPN")) {
        $.ajax({
            url: "GetCurrent",
            type: 'GET',
            dataType: 'json',
            success: function (data1) {
                // Process the data coming back
                if (data1 == null || data1 == "") {
                } else {
                    if (data1.includes("_")) {
                        slider_name = data1.replace(/[_][0-9][a-z][a-z][a-z]/g, '');
                        slider_name2 = data1.replace(/\d.*[_]/g, '');
                        real_ten = data1.replace(/[a-z][a-z][a-z]/g, '');
                    } else {
                        // Handle other cases
                    }
                    manual_color = data1;
                }
                colorChange();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                // Handle errors
            }
        });
        colorChange();
    }
}

function sendmsg(e) {

    id_slider = e.target.id;
    //console.log(id_slider);
    //var st = document.getElementById(event.srcElement.id + "_col").style

    var index = marked.indexOf(id_slider);
    var sum_total = parseInt(document.getElementById('sumAll').innerHTML)
    var current_count = parseInt(document.getElementById('curr_val').innerHTML)
    //console.log(id_slider);
    var color = document.getElementById(id_slider + "_col").style.backgroundColor;
    /*st.backgroundColor = "gray";*/
    //alert(index);
    var sum_top_add = sum_total + 1;
    var sum_top_subt = sum_total - 1;
    var curr_top_subt = current_count - 1;

    if (marked.includes(id_slider)) {

        if (index != -1) {

            marked.splice(index, 1);
            document.getElementById('sumAll').innerHTML = sum_top_add;

        }
        //alert(marked);
    }
    else {
        if (sum_total == current_count) {
            document.getElementById('sumAll').innerHTML = sum_top_subt;
            document.getElementById('curr_val').innerHTML = curr_top_subt;
            document.getElementById(id_slider + "_col").style.backgroundColor = "lightblue";

            marked.push(id_slider);
        }
        else {
            if (current_count != 0 && current_count < sum_total && color == "lime") {
                document.getElementById('sumAll').innerHTML = sum_top_subt;
                document.getElementById(id_slider + "_col").style.backgroundColor = "lightblue";
                document.getElementById('curr_val').innerHTML = curr_top_subt;
                marked.push(id_slider);
            }
            else if (current_count != 0 && current_count < sum_total) {
                document.getElementById('sumAll').innerHTML = sum_top_subt;
                document.getElementById(id_slider + "_col").style.backgroundColor = "lightblue";
                marked.push(id_slider);
            }

            else if (current_count == 0) {
                document.getElementById('sumAll').innerHTML = sum_top_subt;
                document.getElementById(id_slider + "_col").style.backgroundColor = "lightblue";
                
                marked.push(id_slider);
            }
            else {
                document.getElementById('sumAll').innerHTML = sum_top_subt;
                document.getElementById(id_slider + "_col").style.backgroundColor = "lightblue";
                document.getElementById('curr_val').innerHTML = curr_top_subt;
                marked.push(id_slider);
            }

        }

    }

}

function clear() {
    const ManualInput = document.getElementById('addNum');

    // 👇️ clear input field
    if (currentPath.endsWith("Category/SelectedPN")) {
        ManualInput.value = '';
    }
}

function sendState(data) {
    var dataToSend = '';
    if(manual_color != null && manual_color.includes("_")) {
        dataToSend = manual_color.replace(/[_][0-9]/g, '');
    }
    else {
        dataToSend = manual_color
    }
    frontend_data.A1 = data;
    frontend_data.A2 = document.getElementById('part_no').innerHTML;
    frontend_data.A3 = dataToSend;
    $.ajax({
        type: "POST",
        url: "http://localhost:3000/",
        data: JSON.stringify(frontend_data),
        contentType: "json",
        success: function (data) {
            console.log("Data sent successfully");
        },
        error: function (err) {
            console.error("Error sending data:", err);
        }
    });
}

function colorChange() {
    check = parseInt(document.getElementById("curr_val").innerHTML);
    $.ajax({
        url: "SendResult",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var test = data.localeCompare('present');
            var recv_dat = data.replace("t ", 't')

            var new_manual_color1 = manual_color.replace(/[_][0-9]/g, '');
            if (data != null && marked.includes(manual_color)) {
                document.getElementById("top_color").style.backgroundColor = "lime";
                document.getElementById('topUpdate').innerHTML = new_manual_color1;
            }
            else if (data != null && data == "present" && manual_color.includes("_")) {
                var new_manual_color = manual_color.replace(/[_][0-9]/g, '');
                if (document.getElementById(manual_color + "_col").style.backgroundColor == "lime") {
                    document.getElementById(manual_color + "_col").style.backgroundColor = "lime";
                    document.getElementById("top_color").style.backgroundColor = "lime";
                    document.getElementById('topUpdate').innerHTML = new_manual_color;
                }
                else {
                    document.getElementById(manual_color + "_col").style.backgroundColor = "lime";
                    document.getElementById("top_color").style.backgroundColor = "lime";
                    document.getElementById("curr_val").innerHTML = check + 1;
                    document.getElementById('topUpdate').innerHTML = new_manual_color;
                }
                sendState(data);
            }
            else if (data != null && test == 0) {
                if (manual_color !== "") {

                    if (document.getElementById(manual_color + "_col").style.backgroundColor == "lime") {
                        document.getElementById(manual_color + "_col").style.backgroundColor = "lime";
                        document.getElementById("top_color").style.backgroundColor = "lime";
                        document.getElementById('topUpdate').innerHTML = manual_color;
                    }
                    else {
                        document.getElementById(manual_color + "_col").style.backgroundColor = "lime";
                        document.getElementById("top_color").style.backgroundColor = "lime";
                        document.getElementById("curr_val").innerHTML = check + 1;
                        document.getElementById('topUpdate').innerHTML = manual_color;
                    }
                    sendState(data);
                }
            }
            else if (data != null && data == "absent") {

                document.getElementById('topUpdate').innerHTML = manual_color;
                document.getElementById("top_color").style.backgroundColor = "red";

                sendState(data);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(xhr.status);
        }
    });

}

$(document).ready(function () {
    if (window.location.href.endsWith("/Category/LogView")) {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0');
        var yyyy = today.getFullYear();

        var formattedDate = mm + '/' + dd + '/' + yyyy;

        $("#startDate").val(formattedDate);
        $("#endDate").val(formattedDate);
    }



    const toggle = document.getElementById("Upload"),
        closeModal = document.getElementById("closeModal"),
        uploadModal = document.getElementById("modal-opened");

    toggle.addEventListener("click", () => {
        uploadModal.style.display = "flex";
    });

    closeModal.addEventListener("click", () => {
        uploadModal.style.display = "none";
    });
});