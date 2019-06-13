$(function () {
    "use strict";
    
    //Sales chart
    var ctx = document.getElementById( "sales-chart" );
    ctx.height = 230;
    var myChart = new Chart( ctx, {
        type: 'line',
        data: {
            labels: [ "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "فروردین" ],
            type: 'line',
            defaultFontFamily: 'Montserrat',
            datasets: [ {
                label: "ریاضی",
                data: [ 10, 20, 18, 16, 19, 16, 14 ],
                backgroundColor: 'transparent',
                borderColor: 'rgba(220,53,69,0.75)',
                borderWidth: 3,
                pointStyle: 'circle',
                pointRadius: 5,
                pointBorderColor: 'transparent',
                pointBackgroundColor: 'rgba(220,53,69,0.75)',
                    }, {
                label: "شیمی",
                data: [ 12, 16, 20, 14, 17, 20, 12 ],
                backgroundColor: 'transparent',
                borderColor: 'rgba(40,167,69,0.75)',
                borderWidth: 3,
                pointStyle: 'circle',
                pointRadius: 5,
                pointBorderColor: 'transparent',
                pointBackgroundColor: 'rgba(40,167,69,0.75)',
                    } ]
        },
        options: {
            responsive: true,

            tooltips: {
                mode: 'index',
                titleFontSize: 14,
                titleFontColor: '#fff',
                bodyFontColor: '#fff',
                backgroundColor: 'rgba(43, 59, 75, 1)',
                titleFontFamily: 'Montserrat',
                bodyFontFamily: 'Montserrat',
                cornerRadius: 3,
                intersect: false,
            },
            legend: {
                display: true,
                labels: {
                    usePointStyle: true,
                    fontFamily: 'Montserrat',
                },
            },
            scales: {
                xAxes: [ {
                    display: true,
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },
                    scaleLabel: {
                        display: false,
                        labelString: 'Month'
                    }
                        } ],
                yAxes: [ {
                    display: true,
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },
                    scaleLabel: {
                        display: true,
                        labelString: 'Value'
                    }
                        } ]
            },
            title: {
                display: false,
                text: 'Normal Legend'
            }
        }
    } );
    
    //doughut chart
    var ctx = document.getElementById( "doughutChart" );
    ctx.height = 230;
    var myChart = new Chart( ctx, {
        type: 'doughnut',
        data: {
            datasets: [ {
                data: [ 45, 25, 20, 10, 3 ],
                backgroundColor: [
                                    "rgba(255,204,0,1)",
                                    "rgba(8,174,204,1)",
                                    "rgba(253,100,10,1)",
                                    "rgba(8,94,204,1)",
                                    "rgba(132,8,204,1)"
                                ],
                hoverBackgroundColor: [
                                    "rgba(211,166,2,1)",
                                    "rgba(6,132,154,1)",
                                    "rgba(211,79,2,1)",
                                    "rgba(6,72,154,1)",
                                    "rgba(100,6,154,1)"
                                ]

                            } ],
            labels: [
                            "ریاضی",
                            "علوم",
                            "شیمی",
                            "فیزیک",
                            "ورزش"
                        ]
        },
        options: {
            responsive: true,
            tooltips:{
				intersect: false,
                backdropColor: 'rgba(255, 120, 22, 1)',
                yPadding: 10,
                xPadding: 10,
                caretSize: 8,
                backgroundColor: 'rgba(43, 59, 75, 1)',
                titleFontColor: 'rgb(255,255,255)' ,
                bodyFontColor: 'rgb(255,255,255)' ,
                borderColor: 'rgba(25,33,43,1)',
                borderWidth: 1
            }
        }
    } );
    
    //radar chart
    var ctx = document.getElementById( "radarChart" );
    ctx.height = 230;
    var myChart = new Chart( ctx, {
        type: 'radar',
        data: {
            labels: [ "تمرکز", "پرسش شفاهی" , "پرسش ماهانه", "کتبی", "انظباط", "ورزش"],
            datasets: [
                {
                    label: "زیست",
                    data: [ 14, 16, 13, 18,17, 15, 19 ],
                    borderColor: "rgba(43, 59, 75, 1)",
                    borderWidth: "1",
                    backgroundColor: "rgba(43, 59, 75, .7)"
                            },
                {
                    label: "فیزیک",
                    data: [ 15, 12, 18, 20, 13, 18, 15 ],
                    borderColor: "rgba(0, 123, 255, 0.7",
                    borderWidth: "1",
                    backgroundColor: "rgba(0, 123, 255, 0.5)"
                            }
                        ]
        },
        options: {
            legend: {
                position: 'top'
            },
            scale: {
                ticks: {
                    beginAtZero: true
                }
            }
        }
    } );
    
    
    // single bar chart
    var ctx = document.getElementById( "singelBarChart" );
    ctx.height = 230;
    var myChart = new Chart( ctx, {
        type: 'bar',
        data: {
            labels: [ "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "مهر", "آبان" ],
            datasets: [
                {
                    label: "ریاضی",
                    data: [ 40, 55, 75, 81, 56, 55, 40 ],
                    borderColor: "rgba(132,8,204,1)",
                    borderWidth: "0",
                    backgroundColor: "rgba(132,8,204,1)"
                },
                {
                    label: "شیمی",
                    data: [ 28, 48, 40, 19, 86, 27, 90 ],
                    borderColor: "rgba(8,174,204,1)",
                    borderWidth: "0",
                    backgroundColor: "rgba(8,174,204,1)"
                            }
                        ]
        },
        options: {
            scales: {
                yAxes: [ {
                    ticks: {
                        beginAtZero: true
                    }
                                } ]
            },
            title: {
                display: true,
                text: 'Hii Everyone'
            },
            tooltips:{
                position: 'nearest',
				mode: 'index',
				intersect: false,
                backdropColor: 'rgba(255, 120, 22, 1)',
                yPadding: 10,
                xPadding: 10,
                caretSize: 8,
                backgroundColor: 'rgba(43, 59, 75, 1)',
                titleFontColor: 'rgb(255,255,255)' ,
                bodyFontColor: 'rgb(255,255,255)' ,
                borderColor: 'rgba(25,33,43,1)',
                borderWidth: 1
            }
        }
    } );
    
    
    
} )( jQuery );