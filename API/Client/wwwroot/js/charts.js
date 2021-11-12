$.ajax({
    url: "https://localhost:44349/API/Employees/Gender"
}).done((result) => {
    console.log(result);
    var option = {
        series: [],
        labels: [],
        chart: {
            type: 'donut',
            height: 400,
            offsetY: 40
        },
        plotOptions: {
            radialBar: {
                offsetY: -30
            }
        },
        legend: {
            show: true,
            position: 'left',
            containerMargin: {
                right: 0
            }
        },
        title: {
            text: 'Gender Chart',
            style: {
                fontSize: '20px'
            }
        }
    };
    result.map(x => {
        option.series.push(Math.round(x.value));
    });
    result.map(x => {
        if (x.gender == 0) {
            option.labels.push("Male");
        } else {
            option.labels.push("Female");
        }
    });
    var chart = new ApexCharts(document.querySelector("#myPieChart"), option);
    chart.render();
});

$.ajax({
    url: "https://localhost:44349/API/Employees/Role"
}).done((result) => {
    console.log(result);
    var option = {
        series: [],
        labels: [],
        chart: {
            type: 'pie',
            height: 400,
            offsetY: 40
        },
        plotOptions: {
            radialBar: {
                offsetY: -30
            }
        },
        legend: {
            show: true,
            position: 'left',
            containerMargin: {
                right: 0
            }
        },
        title: {
            text: 'Role Chart',
            style: {
                fontSize: '20px'
            }
        }
    };
    result.map(x => {
        option.series.push(Math.round(x.value));
    });
    result.map(x => {
        option.labels.push(x.roleName);
    });
    var chart = new ApexCharts(document.querySelector("#myPieChart1"), option);
    chart.render();
});

$.ajax({
    url: "https://localhost:44349/API/Employees/Salary"
}).done((result) => {
    console.log(result);
    var option = {
        series: [{
            name: 'Employee',
            data: result.map(x => x.value)
        }],
        chart: {
            type: 'bar',
            height: 360,
            offsetY: 20
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                columnWidth: '20%',
                horizontal: false
            }
        },
        dataLabels: {
            enabled: false
        },
        xaxis: {
            categories: [],
        },
        title: {
            text: 'Salary Chart',
            align: 'left',
            style: {
                fontSize: '20px'
            }
        },
        legend: {
            offsetY: -50,
            position: 'top',
            horizontalAlign: 'right'
        }
    };
    result.map(x => {
        option.xaxis.categories.push(x.label);
    });
    var chart = new ApexCharts(document.querySelector("#myAreaChart"), option);
    chart.render();
});



