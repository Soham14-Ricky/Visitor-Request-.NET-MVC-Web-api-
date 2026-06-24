$(document).ready(function() {

            $("#adminGrid").jqGrid({

                                datatype: "local",
                                data: requestData,
                                sortname: "RequestId",
                                sortorder: "desc",
                                sortable: true,

                                colModel: [

                                    { label: "Id", name: "RequestId", key: true, width: 70, sorttype: "integer" },

                                    { label: "Visitor Name", name: "VisitorName", width: 150, sorttype: "text", searchoptions: { sopt: ["cn"] } },

                                    { label: "Mobile", name: "MobileNumber", width: 120, sorttype: "text", searchoptions: { sopt: ["cn"] } },

                                    { label: "Company", name: "CompanyName", width: 150, sorttype: "text", searchoptions: { sopt: ["cn"] } },

                                    { label: "Person To Meet", name: "PersonToMeet", width: 150, sorttype: "text", searchoptions: { sopt: ["cn"] } },

                                    { label: "Visit Date", name: "VisitDate", width: 120, sorttype: "date", searchoptions: { sopt: ["eq", "gt", "lt"] } },

                                    {
                                        label: "Status", name: "Status", width: 100, sorttype: "text",
                                        stype: "select",
                                        searchoptions: {
                                            sopt: ["eq"],
                                            value: ":All;Pending:Pending"    // admin grid only ever shows Pending
                                        }
                                    },

                                    {
                                        label: "Actions",
                                        name: "Actions",
                                        width: 220,
                                        formatter: actionFormatter,
                                        search: false,
                                        sortable: false
                                    }
                                ],

                                viewrecords: true,
                                width: 1150,
                                height: 400,
                                rowNum: 10,
                                pager: "#adminPager",
                                caption: "Pending Visitor Requests"
                            });

            $("#adminGrid").jqGrid("filterToolbar", {
                                searchOnEnter: false,
                                defaultSearch: "cn"
                            });
        });

// ================= ACTION BUTTONS =================

function actionFormatter(cellvalue, options, rowObject) {

    return `
                        <button class='btn btn-sm btn-success me-1'
                                onclick='approveRequest(${rowObject.RequestId})'>
                            Approve
                        </button>

                        <button class='btn btn-sm btn-danger'
                                onclick='rejectRequest(${rowObject.RequestId})'>
                            Reject
                        </button>
                    `;
}

// ================= APPROVE =================

function approveRequest(requestId) {
 
    if (confirm("Approve this request?")) {
 
        var token =

            $('input[name="__RequestVerificationToken"]').val();
 
        var form =

            $('<form method="post" action="/Admin/Approve"></form>');
 
        form.append(

            '<input type="hidden" name="__RequestVerificationToken" value="' + token + '" />'

        );
 
        form.append(

            '<input type="hidden" name="id" value="' + requestId + '" />'

        );
 
        $('body').append(form);
 
        form.submit();

    }

}


// ================= REJECT (FIXED FOR BOOTSTRAP 5) =================

function rejectRequest(requestId) {

    console.log("Reject clicked:", requestId);

    $("#rejectRequestId").val(requestId);

    const modalElement = document.getElementById('rejectModal');

    const modal = new bootstrap.Modal(modalElement);

    modal.show();
} 