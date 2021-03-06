#pragma checksum "C:\Projects\Beau.Fullstack\chendan\ChendanKelly\ChendanKelly\Views\Home\DataSource.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0830f6e88c852b99946457213fe1386ef163e40f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_DataSource), @"mvc.1.0.view", @"/Views/Home/DataSource.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/DataSource.cshtml", typeof(AspNetCore.Views_Home_DataSource))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Projects\Beau.Fullstack\chendan\ChendanKelly\ChendanKelly\Views\_ViewImports.cshtml"
using ChendanKelly;

#line default
#line hidden
#line 2 "C:\Projects\Beau.Fullstack\chendan\ChendanKelly\ChendanKelly\Views\_ViewImports.cshtml"
using ChendanKelly.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0830f6e88c852b99946457213fe1386ef163e40f", @"/Views/Home/DataSource.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"13d5adcc34f9525c7b600a27605207bbd2339239", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_DataSource : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Projects\Beau.Fullstack\chendan\ChendanKelly\ChendanKelly\Views\Home\DataSource.cshtml"
  
    ViewData["Title"] = "Data Source";

#line default
#line hidden
            BeginContext(47, 539, true);
            WriteLiteral(@"
<style>
    .loader-background {
        width:100%;
        height: 750px;
        background-color: #9b99993d;
        position: relative;
        padding-top:250px;
    }
    .loader {
        border: 16px solid #f3f3f3; /* Light grey */
        border-top: 16px solid #3498db; /* Blue */
        border-radius: 50%;
        width: 120px;
        height: 120px;
        animation: spin 2s linear infinite;
        margin-left: auto;
        margin-right: auto;
    }
    button {
        margin-top: 2px;
    }

");
            EndContext();
            BeginContext(587, 27087, true);
            WriteLiteral(@"@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}
</style>
<div class=""row"" id=""datasource"">

    <div class=""col-lg-3"" style=""padding:5px;"">
        <div style=""height:90px"">
            <label for=""start"">文件对应的日期</label>
            <input type=""date"" id=""file-date-picker"" name=""trip""
                   value=""2018-07-22""
                   min=""2018-01-01"" max=""2018-12-31"" />
            <div>
                <button type=""button"" class=""btn btn-success""
                        onclick=""uploadFile()"">
                    根据名称文件名添加数据
                </button>
                <button type=""button"" class=""btn btn-primary btn-sm""
                        onclick=""uploadBaobeiFile()"">
                    +宝贝报表
                </button>
                <button type=""button"" class=""btn btn-secondary btn-sm""
                        onclick=""uploadOrderFile()"">
                    +订单报表
                </button>
                <button type=""but");
            WriteLiteral(@"ton"" class=""btn btn-success btn-sm""
                        onclick=""uploadFeeFile()"">
                    +费用报表
                </button>
                <button type=""button"" class=""btn btn-danger btn-sm""
                        onclick=""uploadShouruFile()"">
                    +收入报表
                </button>
                <button type=""button"" class=""btn btn-success btn-sm""
                        onclick=""uploadUnitPriceFile()"">
                    +宝贝单价
                </button>
            </div>
            <input id=""file-picker"" type=""file"" style=""display:none;"" />
        </div>
        <div>
            <div id=""fileGrid"" style=""height:600px;min-height: 600px;width:100%;margin-top:5px;""
                    class=""ag-theme-balham""></div>
        </div>
    </div>

    <div class=""col-lg-9"" style=""padding:5px;"">
        <div style=""height:50px;position:relative;"">
            <button style=""position:absolute;bottom:0px;"" id=""delete-selected-items"" onclick=""deleteSelectedItems");
            WriteLiteral(@"()"">删除表格中内容</button>
       
        </div>
        <div id=""datasourceGrid"" style=""height:640px;min-height: 600px;width:100%;margin-top:5px;""
             class=""ag-theme-balham""></div>
    </div>

    <div class=""loader-background"">
        <div class=""loader""></div>
    </div>

</div>

<script src=""https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.8.0/xlsx.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.8.0/jszip.js""></script>

<script type=""text/javascript"" charset=""utf-8"">
    let state = {
        selectedFileType: '',
        selectionRow: null,
        selectedFile: undefined
    }

    $(document).ready(function () {
        $('#file-date-picker').val(formatDate(new Date()));
        loading(false);
    });
    setGridHeight();
    $(window).resize(function () {
        setGridHeight();
    });
    
    var fileGridColumnDefs = [
        {
            headerName: ""Date"", field: ""date"", width: 90, rowGroup: true, hide: true
        },
        {");
            WriteLiteral(@"
            headerName: ""FileName"", field: ""fileName"", width: 300
        }
    ];
    var fileGridOptions = {
        columnDefs: fileGridColumnDefs,
        rowSelection: 'single',
        onSelectionChanged: onFileGridSelectionChanged,
        enableSorting: true,
        groupMultiAutoColumn: true,
        enableColResize: true
    };
    var fileGrid = document.querySelector('#fileGrid');
    new agGrid.Grid(fileGrid, fileGridOptions);
    refreshFileGrid();


    var datasourceGridOptions = {
        rowSelection: 'single',
        columnDefs: [],
        enableSorting: true,
        enableColResize: true
    };
    var datasourceGrid = document.querySelector('#datasourceGrid');
    new agGrid.Grid(datasourceGrid, datasourceGridOptions);
    refreshDatasourceGrid();

    function setGridHeight() {
        $('#fileGrid')[0].style.height = (window.innerHeight - 200) + 'px';
        $('#datasourceGrid')[0].style.height = (window.innerHeight - 160) + 'px';
    }
    function c");
            WriteLiteral(@"learSlash(text) {
        if (!((typeof text) == 'string'))
            return text;
        if (text === undefined || text === null)
            return text;
        if (text.includes('='))
            text = text.replace('=', '');
        if (text.includes('\""'))
            text = text.replace('\""', '');
        if (text.includes('""'))
            text = text.replace('""', '');
        text = text.replace('\t', '').trim();
        return text;
    }
    function uploadFile() {
        state.selectedFileType = '';
        $('#file-picker').click();
        $(""#file-picker"").change(function (e) {
            let file = e.currentTarget.files[0];
            document.getElementById(""file-picker"").value = """";
            if (file != undefined && file.name != undefined) {
                if (file.name.includes('fee_')) {
                    let dateStr = file.name.replace('fee_', '').replace('_', '-').replace('_', '-').replace('.csv', '');
                    uploadFeeFileCallback(file, dat");
            WriteLiteral(@"eStr);
                }
                if (file.name.includes('settle_')) {
                    let dateStr = file.name.replace('settle_', '').replace('_', '-').replace('_', '-').replace('.csv', '');
                    uploadShouruFileCallback(file, dateStr);
                }
                if (file.name.includes('price_')) {
                    let dateStr = file.name.replace('price_', '').replace('_', '-').replace('_', '-').replace('.csv', '');
                    uploadUnitPriceFileCallback(file, dateStr);
                }
                if (file.name.includes('日宝贝报表')) {
                    let year = (new Date()).getFullYear();
                    let monthNumber = file.name.split('月')[0];
                    let dayNumber = file.name.split('月')[1].split('日')[0];
                    let dateStr = year + '-' + monthNumber + '-' + dayNumber;
                    uploadBaobeiFileCallback(file, dateStr);
                }
                if (file.name.includes('日订单报表')) {
            ");
            WriteLiteral(@"        let year = (new Date()).getFullYear();
                    let monthNumber = file.name.split('月')[0];
                    let dayNumber = file.name.split('月')[1].split('日')[0];
                    let dateStr = year + '-' + monthNumber + '-' + dayNumber;
                    uploadOrderFileCallback(file, dateStr);
                }
            }
        });
    }
    function uploadBaobeiFile() {
        state.selectedFileType = 'Baobei';
        $('#file-picker').click();
        $(""#file-picker"").change(function (e) {
            if (state.selectedFileType === 'Baobei') {
                state.selectedFileType = '';
                let file = e.currentTarget.files[0];
                document.getElementById(""file-picker"").value = """";
                uploadBaobeiFileCallback(file, $('#file-date-picker')[0].value);
            }
        });
    }
    function uploadBaobeiFileCallback(file, date) {
        if (file === undefined) {
            return;
        }
        var reade");
            WriteLiteral(@"r = new FileReader();
        reader.onload = function () {
            try {
                loading(true);
                var fileJson = csvJSON(reader.result);
                let newBaobeis = [];
                for (let i = 0; i < fileJson.length; i++) {
                    let orderId = fileJson[i][""订单编号""] == undefined ? fileJson[i][""\""订单编号\""""] : fileJson[i][""订单编号""];
                    let baobeiExternalId = fileJson[i][""外部系统编号""] == undefined ? fileJson[i][""\""外部系统编号\""""] : fileJson[i][""外部系统编号""];
                    let quantity = fileJson[i][""购买数量""] == undefined ? fileJson[i][""\""购买数量\""""] : fileJson[i][""购买数量""];
                    let baobeiTitle = fileJson[i][""标题""] == undefined ? fileJson[i][""\""标题\""""] : fileJson[i][""标题""];
                    newBaobeis.push({
                        orderId: clearSlash(orderId),
                        baobeiExternalId: clearSlash(baobeiExternalId),
                        quantity: clearSlash(quantity),
                        baobeiTitle: clearSlash(ba");
            WriteLiteral(@"obeiTitle)
                    });
                }
                uploadBaobei(date, newBaobeis, file.name);
            }
            catch{
                alert('上传文件失败');
                loading(false);
            }
        };
        reader.readAsText(file, 'gb2312');
    }
    function uploadOrderFile() {
        state.selectedFileType = 'Order';
        $('#file-picker').click();
        $(""#file-picker"").change(function (e) {
            if (state.selectedFileType === 'Order') {
                state.selectedFileType = '';
                let file = e.currentTarget.files[0];
                document.getElementById(""file-picker"").value = """";
                uploadOrderFileCallback(file, $('#file-date-picker')[0].value);
            }
        });
    }
    function uploadOrderFileCallback(file, date) {        
        if (file === undefined) {
            return;
        }
        var reader = new FileReader();
        reader.onload = function () {
            try {
  ");
            WriteLiteral(@"              loading(true);
                var fileJson = csvJSON(reader.result);
                let newOrders = [];
                debugger;
                for (let i = 0; i < fileJson.length; i++) {
                    let orderId = fileJson[i][""订单编号""] == undefined ? fileJson[i][""\""订单编号\""""] : fileJson[i][""订单编号""];
                    let amount = fileJson[i][""总金额""] == undefined ? fileJson[i][""\""总金额\""""] : fileJson[i][""总金额""];
                    newOrders.push({
                        orderId: clearSlash(orderId),
                        amount: clearSlash(amount),
                    });
                }
                uploadOrder(date, newOrders, file.name);
            }
            catch{
                alert('上传文件失败');
                loading(false);
            }
        }
        reader.readAsText(file, 'gb2312');
    }
    function uploadFeeFile() {
        state.selectedFileType = 'Fee';
        $('#file-picker').click();
        $(""#file-picker"").change(function (e) {");
            WriteLiteral(@"
            if (state.selectedFileType === 'Fee') {
                state.selectedFileType = '';
                let file = e.currentTarget.files[0];
                document.getElementById(""file-picker"").value = """";
                uploadFeeFileCallback(file, $('#file-date-picker')[0].value);
            }
        });
    }
    function uploadFeeFileCallback(file, date) {
        if (file === undefined) {
            return;
        }
        var reader = new FileReader();
        reader.onload = function () {
            try {
                loading(true);
                var fileJson = csvJSON(reader.result);
                debugger;
                let newFees = [];
                for (let i = 0; i < fileJson.length; i++) {
                    let partnerOrderId = fileJson[i][""Partner_transaction_id""];
                    let feeType = fileJson[i][""Fee_type""];
                    let feeAmount = fileJson[i][""Fee_amount""];
                    newFees.push({
                    ");
            WriteLiteral(@"    partnerOrderId: clearSlash(partnerOrderId),
                        feeType: clearSlash(feeType),
                        feeAmount: clearSlash(feeAmount)
                    });
                }
                uploadFee(date, newFees, file.name);
            }
            catch{
                alert('上传文件失败');
                loading(false);
            }
        }
        reader.readAsText(file, 'gb2312');
    }
    function uploadShouruFile() {
        state.selectedFileType = 'Shouru';
        $('#file-picker').click();
        $(""#file-picker"").change(function (e) {
            if (state.selectedFileType === 'Shouru') {
                state.selectedFileType = '';
                let file = e.currentTarget.files[0];
                document.getElementById(""file-picker"").value = """";
                uploadShouruFileCallback(file, $('#file-date-picker')[0].value);
            }
        });
    }
    function uploadShouruFileCallback(file, date) {
        if (file === undefi");
            WriteLiteral(@"ned) {
            return;
        }
        var reader = new FileReader();
        reader.onload = function () {
            try {
                loading(true);
                var fileJson = csvJSON(reader.result);
                let newSettles = [];
                debugger;
                for (let i = 0; i < fileJson.length; i++) {
                    let orderId = fileJson[i][""Partner_transaction_id""];
                    let amount = fileJson[i][""Amount""];
                    let settleAmount = fileJson[i][""Settlement""];
                    let settlementTime = fileJson[i][""Settlement_time""];
                    if (amount != undefined && amount !== null)
                        amount = amount.trim()
                    if (settleAmount != undefined && settleAmount !== null)
                        settleAmount = settleAmount.trim()
                    newSettles.push({
                        orderId: clearSlash(orderId),
                        amount: clearSlash(amount),
   ");
            WriteLiteral(@"                     settleAmount: clearSlash(settleAmount),
                        settlementTime: clearSlash(settlementTime)
                    });
                }
                uploadSettle(date, newSettles, file.name);
            }
            catch (e) {
                console.log(e);
                alert('上传文件失败');
                loading(false);
            }
        }
        reader.readAsText(file, 'gb2312');
    }
    function uploadUnitPriceFile() {
        state.selectedFileType = 'UnitPrice';
        $('#file-picker').click();
        $(""#file-picker"").change(function (e) {
            if (state.selectedFileType === 'UnitPrice') {
                state.selectedFileType = '';
                let file = e.currentTarget.files[0];
                document.getElementById(""file-picker"").value = """";
                uploadUnitPriceFileCallback(file, $('#file-date-picker')[0].value);
            }
        });
    }
    function uploadUnitPriceFileCallback(file, date) {
");
            WriteLiteral(@"        if (file === undefined) {
            return;
        }
        var reader = new FileReader();
        reader.onload = function () {
            try {
                loading(true);
                var fileJson = csvJSON(reader.result);
                let newPrices = [];
                debugger;
                for (let i = 0; i < fileJson.length; i++) {
                    let baobeiId = fileJson[i][""BaobeiId""];
                    let baobeiTitle = fileJson[i][""BaobeiTitle""];
                    let unitPriceInEuro = fileJson[i][""UnitPriceInEuro""];
                    if (unitPriceInEuro !== undefined)
                        unitPriceInEuro = parseFloat(unitPriceInEuro);
                    newPrices.push({
                        baobeiId: baobeiId,
                        baobeiTitle: baobeiTitle,
                        unitPriceInEuro: unitPriceInEuro
                    });
                }
                uploadPrice(date, newPrices, file.name);
            }
      ");
            WriteLiteral(@"      catch{
                alert('上传文件失败');
                loading(false);
            }
        }
        reader.readAsText(file, 'utf-8');
    }

    function refreshFileGrid() {
        agGrid.simpleHttpRequest({ url: '/DataSource/GetAllFiles' }).then(function (data) {
            let newData = [];
            for (let i = 0; i < data.length; i++) {
                data[i].date = data[i].date.slice(0, 10);
                newData.push(data[i]);
            }
            fileGridOptions.api.setRowData(data);
            fileGridOptions.api.sizeColumnsToFit();
        });
    }

    function onFileGridSelectionChanged() {
        var selectedRows = fileGridOptions.api.getSelectedRows();
        state.selectionRow = selectedRows[0];
        refreshDatasourceGrid();
        //alert(JSON.stringify(selectedRows[0]));
    }

    function refreshDatasourceGrid() {
        if (state.selectionRow != null) {
            switch (state.selectionRow.fileType) {
                case 'Baob");
            WriteLiteral(@"ei':
                    debugger;
                    datasourceGridOptions.api.setColumnDefs([
                        {
                            headerName: ""订单编号"", field: ""orderId""
                        },
                        {
                            headerName: ""外部系统编号"", field: ""baobeiExternalId""
                        },
                        {
                            headerName: ""购买数量"", field: ""quantity""
                        },
                        {
                            headerName: ""标题"", field: ""baobeiTitle""
                        }
                    ]);
                    loading(true);
                    agGrid.simpleHttpRequest({ url: '/DataSource/GetBaobeis?date=' + state.selectionRow.date }).then(function (data) {
                        loading(false);
                        datasourceGridOptions.api.setRowData(data);
                    });
                    break;
                case 'Order':
                    datasourceGridOp");
            WriteLiteral(@"tions.api.setColumnDefs([
                        {
                            headerName: ""订单编号"", field: ""orderId""
                        },
                        {
                            headerName: ""总金额"", field: ""amount""
                        }
                    ]);
                    loading(true);
                    agGrid.simpleHttpRequest({ url: '/DataSource/GetOrders?date=' + state.selectionRow.date }).then(function (data) {
                        loading(false);
                        datasourceGridOptions.api.setRowData(data);
                    });
                    break;
                case 'Fee':
                    datasourceGridOptions.api.setColumnDefs([
                        {
                            headerName: ""订单编号"", field: ""partnerOrderId""
                        },
                        {
                            headerName: ""Fee Type"", field: ""feeType""
                        },
                        {
                           ");
            WriteLiteral(@" headerName: ""Fee Amount"", field: ""feeAmount""
                        }
                    ]);
                    loading(true);
                    agGrid.simpleHttpRequest({ url: '/DataSource/GetFees?date=' + state.selectionRow.date }).then(function (data) {
                        loading(false);
                        datasourceGridOptions.api.setRowData(data);
                    });
                    break;
                case 'Price':
                    datasourceGridOptions.api.setColumnDefs([
                        {
                            headerName: ""宝贝编号"", field: ""baobeiId""
                        },
                        {
                            headerName: ""宝贝标题"", field: ""baobeiTitle""
                        },
                        {
                            headerName: ""宝贝单价"", field: ""unitPriceInEuro""
                        }
                    ]);
                    loading(true);
                    agGrid.simpleHttpRequest({ url: '/DataSourc");
            WriteLiteral(@"e/GetPrices?date=' + state.selectionRow.date }).then(function (data) {
                        loading(false);
                        datasourceGridOptions.api.setRowData(data);
                    });
                    break;
                case 'Settle':
                    datasourceGridOptions.api.setColumnDefs([
                        {
                            headerName: ""订单编号"", field: ""orderId""
                        },
                        {
                            headerName: ""应总收入"", field: ""amount""
                        },
                        {
                            headerName: ""实际入账"", field: ""settleAmount""
                        },
                        {
                            headerName: ""入账时间"", field: ""settlementTime""
                        }
                    ]);
                    loading(true);
                    agGrid.simpleHttpRequest({ url: '/DataSource/GetSettles?date=' + state.selectionRow.date }).then(function (data) {
   ");
            WriteLiteral(@"                     loading(false);
                        datasourceGridOptions.api.setRowData(data);
                    });
                    break;
            }
        }
        else {
            datasourceGridOptions.api.setColumnDefs([
            ]);
            datasourceGridOptions.api.setRowData([]);
        }

        datasourceGridOptions.api.sizeColumnsToFit();
    }

    function uploadFee(date, jsonValue, fileName) {
        fetch('/DataSource/InsertDataToFeeTable', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ date, newFees: jsonValue, fileName })
        }).then(e => {
            loading(false);
            if (e.status === 200) {
                refreshFileGrid();
            }
            else {
                alert('上传文件失败');
            }
            }).catch(e => {
                alert('上传文件失败')");
            WriteLiteral(@";
            loading(false);
        });
    }

    function uploadPrice(date, jsonValue, fileName) {
        fetch('/DataSource/InsertDataToPriceTable', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ date, newPrices: jsonValue, fileName })
        }).then(e => {
            loading(false);
            if (e.status === 200) {
                refreshFileGrid();
            }
            else {
                alert('上传文件失败');
            }
        }).catch(e => {
            loading(false);
        });
    }

    function uploadBaobei(date, jsonValue, fileName) {
        fetch('/DataSource/InsertDataToBaobeiTable', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ date, newBaobei");
            WriteLiteral(@"s: jsonValue, fileName })
        }).then(e => {
            loading(false);
            if (e.status === 200) {
                refreshFileGrid();
            }
            else {
                alert('上传文件失败');
            }
            }).catch(e => {
                alert('上传文件失败');
            loading(false);
        });
    }

    function uploadOrder(date, jsonValue, fileName) {
        fetch('/DataSource/InsertDataToOrderTable', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                date,
                newOrders: jsonValue,
                fileName
            })
        }).then(e => {
            loading(false);
            if (e.status === 200) {
                refreshFileGrid();
            }
            else {
                alert('上传文件失败');
            }
        }).catch(e => {
            alert('上");
            WriteLiteral(@"传文件失败');
            loading(false);
        });
    }

    function uploadSettle(date, jsonValue, fileName) {
        debugger;
        fetch('/DataSource/InsertDataToSettleTable', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                date,
                newSettles: jsonValue,
                fileName
            })
        }).then(e => {
            loading(false);
            if (e.status === 200) {
                refreshFileGrid();
            }
            else {
                alert('上传文件失败');
            }
        }).catch(e => {
            alert('上传文件失败');
            loading(false);
        });
    }

    function deleteSelectedItems() {
        if (state.selectionRow != null) {
            let fetchUrl = '/DataSource/';
            if (state.selectionRow.fileType == 'Order') {
                fetchUr");
            WriteLiteral(@"l = fetchUrl + 'DeleteDataFromOrderTableAsync';
            }
            else if (state.selectionRow.fileType == 'Baobei') {
                fetchUrl = fetchUrl + 'DeleteDataFromBaobeiTableAsync';
            }
            else if (state.selectionRow.fileType == 'Fee') {
                fetchUrl = fetchUrl + 'DeleteDataFromFeeTableAsync';
            }
            else if (state.selectionRow.fileType == 'Price') {
                fetchUrl = fetchUrl + 'DeleteDataFromPriceTableAsync';
            }
            else if (state.selectionRow.fileType == 'Settle') {
                fetchUrl = fetchUrl + 'DeleteDataFromSettleTableAsync';
            }
            loading(true);
            fetch(fetchUrl, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(state.selectionRow)
            }).then(e => {
                loading");
            WriteLiteral(@"(false);
                debugger;
                if (e.status === 200) {
                    refreshFileGrid();
                    state.selectionRow = null;
                    refreshDatasourceGrid();
                }
                else {
                    alert('删除数据失败');
                }
            }).catch(e => {
                alert('出现了错误');
                loading(false);
            });
        }
    }

    function csvJSON(csv, path) {
        var lines = csv.split(""\n"");
        var result = [];
        var headers = lines[0].split("","");
        for (var i = 1; i < lines.length; i++) {
            var obj = {};
            var currentline = lines[i].split("","");
            for (var j = 0; j < headers.length; j++) {
                obj[headers[j]] = currentline[j];
            }
            result.push(obj);
        }
        return result;
    }
    function loading(ifOpen) {
        if (ifOpen) {
            $('.loader-background')[0].style.display = '';");
            WriteLiteral(@"
        }
        else {
            $('.loader-background')[0].style.display = 'none';
        }
    }
    function formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    }

</script>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
