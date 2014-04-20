captions = new Array();

//AsciiArtMovieを実現するクラスを定義
function page_Loaded(){
    function MoviePlayer(frames, rendlingElement){
        var _seekPoint = 0;
        var _divArea = rendlingElement;
        var _frames = frames;
        
        //メソッドを定義
        function _move(point){
            _divArea.innerHTML = _frames[point];
            _seekPoint = point;
        }
        function _start(){
            //for(i = _seekPoint; i < _frames.length; i++){
            if(_seekPoint < _frames.length){		
                _move(_seekPoint);
                _seekPoint++;
                
                setTimeout(_start, 30);
            }
            else
            {
                _seekPoint = 0;setTimeout(_start, 30);
            }
        }
    
        //メンバ登録
        this.start = _start
    }

    var player = new MoviePlayer(captions, window.document.getElementById("AsciiArt"));
    player.start();
}