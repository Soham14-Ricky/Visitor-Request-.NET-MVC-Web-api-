$(document).ready(function() {

            

            $("#requestGrid").jqGrid({

                                    datatype: "local",

                                    data: requestData,
                                    sortname: "RequestId",       
                                    sortorder: "desc",           
                                    sortable: true,

                                    colModel: [

                                        {
                                            label: "Id",
                                            name: "RequestId",
                                            key: true,
                                            width: 70,
                                            sorttype: "integer"
                                        },

                                        {
                                            label: "Visitor Name",
                                            name: "VisitorName",
                                            width: 150,
                                            sorttype: "text", 
                                            searchoptions: { sopt: ["cn"] }
                                        },

                                        {
                                            label: "Mobile",
                                            name: "MobileNumber",
                                            width: 120,
                                            sorttype: "text", 
                                            searchoptions: { sopt: ["cn"] }
                                        },

                                        {
                                            label: "Company",
                                            name: "CompanyName",
                                            width: 150,
                                            sorttype: "text", 
                                            searchoptions: { sopt: ["cn"] }
                                        },

                                        {
                                            label: "Visit Date",
                                            name: "VisitDate",
                                            width: 120,
                                            sorttype: "date", 
                                            searchoptions: { sopt: ["eq", "gt", "lt"] }
                                        },

                                        {
                                            label: "Status",
                                            name: "Status",
                                            width: 100,
                                            sorttype: "text",
                                            stype: "select",                             
                                            searchoptions: {
                                                sopt: ["eq"],
                                                value: ":All;Pending:Pending;Approved:Approved;Rejected:Rejected" }
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

                                    width: 1100,

                                    height: 400,

                                    rowNum: 10,

                                    pager: "#requestPager",

                                    caption: "Visitor Requests"

                                });

            $("#requestGrid").jqGrid("filterToolbar", {
                                    searchOnEnter: false,     // filters live as you type
                                    defaultSearch: "cn"       // default: "contains" for text columns
                                });

        });

function actionFormatter(
    cellvalue,
    options,
    rowObject) {
    if (rowObject.Status === "Pending") {
        return `
                            <a href='/VisitorRequest/Edit/${rowObject.RequestId}'
                               class='btn btn-sm btn-warning me-1'>
                               Edit
                            </a>

                            <button
                                class='btn btn-sm btn-danger'
                                onclick='deleteRequest(${rowObject.RequestId})'>
                                Delete
                            </button>
                        `;
    }

    return `
                        <span class='text-muted'>
                            No Actions
                        </span>
                    `;
}

function deleteRequest(requestId) {
    if (confirm(
        "Are you sure you want to delete this request?")) {
        var form =
            $('<form method="post" action="/VisitorRequest/Delete"></form>');

        form.append(
            '<input type="hidden" name="id" value="' + requestId + '" />');

        form.append(
            '@Html.AntiForgeryToken()');

        $('body').append(form);

        form.submit();
    }
} 