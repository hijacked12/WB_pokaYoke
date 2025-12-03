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
name = "";
name2 = "";
let line_no = "";
var model = new Object();
setInterval('stoppedTyping()', 5000);
setInterval('clear()', 30000);
setInterval('refresh()', 1000);


function Line_Name() {
    
    var name = prompt("请输入生产线");
    line_no = name;
    model.X6 = name;
    $.ajax({
        type: "POST",
        url: "Category/Line_No",
        data: model,
        success: function (data) {
            //$("#result").html(data);
            //alert();
        }
    });
    $.ajax({
        type: "POST",
        url: "Category/Index",
        data: model,
        success: function (data) {
            //$("#result").html(data);
            //alert();
        }
    });
    
    
}


var value1 = document.getElementById('addNum');
function stoppedTyping() {
    if (value1.value.length > 0) {
        document.getElementById('AddBtn').disabled = false;
    } else {
        document.getElementById('AddBtn').disabled = true;
    }
}

function verify() {
    if (document.getElementById('addNum').value.length == 0) {
        alert("Put some text in there!");
        return
    }
    else {

    }

}

$(document).ready(function () {
    $('#AddBtn').click(function (e) {
        $(this.form).ajaxSubmit({
            target: false,
            success: function () {
                //alert("success");
                check = parseInt(document.getElementById("curr_val").innerHTML);
                manual_color = document.getElementById('addNum').value;
                ten_num.push(real_ten);
                sleepingFunc();
                //refresh();
                //colorChange();

            }
        });
    });

});

function AddManual() {
    //alert(clicked_id);
    //changedId = clicked_id;
    manual_Input = document.getElementById('addNum').value;
    var model = new Object();
    model.X3 = manual_Input

    $.ajax({
        type: "POST",
        url: "AddManual",
        dataType: "json",

        data: { data1: model },
        success: function (data) {
            alert(data);
        },
        fail: function (errMsg) {
            alert(errMsg);
        }
    });
    alert(manual_Input);
    $.ajax({
        url: "AddManual",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // process the data coming back
            document.getElementById('topUpdate').innerHTML = data;
            alert(data);

        },
    });
    //refresh();

}

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
    //alert("here");

    $.ajax({
        url: "GetCurrent",
        type: 'GET',
        dataType: 'json',
        success: function (data1) {
            // process the data coming back
            if (data1 == null || data1 == "") {
            }
            else {
                if (data1.includes("_")) {

                    name = data1.replace(/[_][0-9][a-z][a-z][a-z]/g, '');
                    name2 = data1.replace(/\d.*[_]/g, '');
                    real_ten = data1.replace(/[a-z][a-z][a-z]/g, '');

                }
                else {
                    //alert("here")
                }
                manual_color = data1;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(xhr.status);
        }
    });
    colorChange();
}

function sendmsg() {

    id_slider = event.srcElement.id;
    var index = marked.indexOf(id_slider);
    var sum_total = parseInt(document.getElementById('sumAll').innerHTML)
    var current_count = parseInt(document.getElementById('curr_val').innerHTML)
    var color = document.getElementById(event.srcElement.id + "_col").style.backgroundColor;

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
            document.getElementById('curr_val').innerHTML = curr_top_subt
            document.getElementById(event.srcElement.id + "_col").style.backgroundColor = "lightblue"
            
            //document.getElementById(event.srcElement.id).disabled = true;
            //document.getElementById(event.srcElement.id).hidden = true;
            marked.push(id_slider);
        }
        else {
            if (current_count != 0 && current_count < sum_total && color == "lime") {
                document.getElementById('sumAll').innerHTML = sum_top_subt;
                document.getElementById(event.srcElement.id + "_col").style.backgroundColor = "lightblue"
                //document.getElementById(event.srcElement.id).disabled = true;
                document.getElementById('curr_val').innerHTML = curr_top_subt
                //document.getElementById(event.srcElement.id).hidden = true;
                marked.push(id_slider);
            }
            else if (current_count != 0 && current_count < sum_total) {
                document.getElementById('sumAll').innerHTML = sum_top_subt;
                document.getElementById(event.srcElement.id + "_col").style.backgroundColor = "lightblue"
                //document.getElementById(event.srcElement.id).disabled = true;
                //document.getElementById(event.srcElement.id).hidden = true;
                marked.push(id_slider);
            }

            else if (current_count == 0) {
                document.getElementById('sumAll').innerHTML = sum_top_subt;
                document.getElementById(event.srcElement.id + "_col").style.backgroundColor = "lightblue"
                //document.getElementById(event.srcElement.id).hidden = true;
                //document.getElementById(event.srcElement.id).dataset.disabled = true;
                //alert(event.srcElement.id);
                marked.push(id_slider);
            }
            else {
                document.getElementById('sumAll').innerHTML = sum_top_subt;
                document.getElementById(event.srcElement.id + "_col").style.backgroundColor = "lightblue"
                //document.getElementById(event.srcElement.id).disabled = true;
                //document.getElementById(event.srcElement.id).hidden = true;
                document.getElementById('curr_val').innerHTML = curr_top_subt
                marked.push(id_slider);
            }
            
        }
        
    }
    
}

function clear() {
    const ManualInput = document.getElementById('addNum');

    // 👇️ clear input field
    ManualInput.value = '';
}

function colorChange() {
    check = parseInt(document.getElementById("curr_val").innerHTML);
    $.ajax({
        url: "SendResult",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // process the data coming back
            //alert(data);
            //check = data;
            var test = data.localeCompare('present');
            var recv_dat = data.replace("t ", 't')
            //var slider_state = document.getElementById(id_slider).hidden;

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
            }
            else if (data != null && test == 0)
            {
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

            }
            else if (data != null && data == "absent") {
                
                    document.getElementById('topUpdate').innerHTML = manual_color;
                    document.getElementById("top_color").style.backgroundColor = "red";
                    //document.getElementById("curr_val").innerHTML = check + 1;
                    //alert("here");
                
                
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(xhr.status);
        }
    });

}

