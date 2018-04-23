$(document).ready(function () {

   
        $.ajax({
            url: "/AudioBook/BookSounds",
            type: "POST",
            datatype: "json",
            data: { id: parseInt($(".BookId").html()) },
            success: function (data) {
                var v = JSON.parse("[" + data + "]");
                SoundList = v[0];

                createElemet(SoundList.length)
                var qlobal = 0;
                var player = $("#jquery_jplayer_1");
                var numberOfChapters = $("#playlist li a").last().attr("tabindex");
                console.log(numberOfChapters)

                function createElemet(countOfsounds) {
                    for (var i = 0; i <= countOfsounds; i++) {
                        $("#playlist").append("<li><i class='fa fa-play-circle myIcon' aria-hidden='true'></i><a href='#' class='chapterLink'  tabindex='" + i + "'>" + $(".BookName").html() + " : Chapter-" + (i + 1) + "</a><i class='fa fa-download' aria-hidden='true'></i></li>");
                    }
                }




                $("#playlist li a").click(function () {
                    var index = $(this).attr("tabindex");
                    qlobal = index;
                    myFunc(qlobal)
                    console.log($("#jquery_jplayer_1 audio").attr("src"))

                });
                $(".jp-next").click(function () {
                    console.log($("#jquery_jplayer_1 audio").attr("src"))
                    if (qlobal < numberOfChapters) {
                        qlobal++;
                        myFunc(qlobal)
                        console.log($("#jquery_jplayer_1 audio").attr("src"))

                    }
                });
                $(".jp-previous").click(function () {
                    console.log($("#jquery_jplayer_1 audio").attr("src"))
                    if (qlobal > 1) {
                        qlobal--;
                        myFunc(qlobal)
                        console.log($("#jquery_jplayer_1 audio").attr("src"))

                    }
                });



                function myFunc(param) {
                    console.log(SoundList[param]["SoundSource"])
                    player.jPlayer({
                        ready: function () {
                            player.jPlayer("setMedia", {
                                mp3: "../../../Uploads/Books/mp3/" + SoundList[param]["SoundSource"]

                            });
                            player.jPlayer("play", 1);
                        },
                        swfPath: "/js",
                        supplied: "mp3",
                    });
                    player.jPlayer("setMedia", {
                        mp3: "../../../Uploads/Books/mp3/" + SoundList[param]["SoundSource"]
                    });
                    player.jPlayer("play", 1);


                    return;
                }

            },

        });

    




});