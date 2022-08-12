(function () {

    const hasMoreComments = (page, limit, total) => {
        const startIndex = (page - 1) * limit + 1;
        return total === 0 || startIndex < total;
    };

    // control variables
    let currentPage = 1;
    const limit = 5;
    let total = 0;

    

    window.addEventListener('scroll', () => {
        const {
            scrollTop,
            scrollHeight,
            clientHeight
        } = document.documentElement;
        
        if (scrollTop + clientHeight >= scrollHeight - 5 &&
            hasMoreComments(currentPage, limit, total)) {
            currentPage++;
            //var myWidgetManager = new abp.WidgetManager('#nav-home');
            //myWidgetManager.refresh();
        }
    }, {
        passive: true
    });
})();

//var commentsList = $("#comments-list");

//$.get("text-editor", function (data) { commentsList.html(data); });

//	//pageElement.className = 'comments-list__page';
	//var list = document.querySelector("p.gfg1");
	//var id = $("#comments-list-pagination").attr("data-content");
	//volo.cmsKit.public.comments.commentPublic.getList("Movie", id).done((res) => {

	//	while (commentssPerPage>i) {
	//		$(".comment").each(function (index, element) {
	//			debugger;
	//			let dom = new DOMParser().parseFromString(element.innerHTML, 'text/html');
	//			var comment = dom.body.firstElementChild
	//			pageElement.append(comment);
	//			i=i+2;
 //           })
	//	}

//var id = $("#comments-list-pagination").attr("data-content");


            //var container = $("#ids");
            //var refreshComponent = function () {
            //    $.get("/Comments/MyViewComponent"+id, function (data) {
            //        console.log(data);
            //        container.html(data);
            //    });
            //};
           // $(function () { window.setInterval(refreshComponent, 1000); });



	//	var id = $("#comments-list-pagination").attr("data-content");
	//	movieMatch.rating.ratingPublic.getCommentsWithRating("Movie", id).done((res) => {

	//		res[i]
	//		i++;
	//	//	})