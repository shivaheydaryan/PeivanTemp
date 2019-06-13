$(function () {
    "use strict";
    
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