
$(function () {

    var skipCount = 0;
    const maxResultCount = 10;
    const defaultImagePath = "default_picture.png";
    const rootImagePath = "/images/host/my-file-container/";
    const l= abp.localization.getResource("MovieMatch");
    getPosts = async () => {
        await $('.loader').fadeIn().promise();
        
        movieMatch.posts.post.getFeed({ userId: `${abp.currentUser.id}`,maxCount:maxResultCount,skipCount}).done(async (res) => {
            await $('.loader').fadeOut().promise();

            renderResults(res.items);
            
            if (res.items.length < maxResultCount) {

                if (res.items.length == 0) {
                    await $('.no-post-data').fadeIn().promise();
                } else {
                    $('.btn-load-more').prop('disabled', true);
                    $('.btn-load-more').text(l('NoMoreData'));
                }
            } 

            if (res.items.length>0)
                await $('.btn-load-more').fadeIn().promise();
            
            skipCount += res.items.length;
           
        });
    }

    renderResults = (posts) => {

        posts.forEach((val, i) => {
            $('#post-list').append(`
                <div class="card myCard" >
                    <div class="card-body">
                       
                        <h5 class="card-title d-flex justify-content-start">
                    <div class="d-flex flex-column">
                   <img class="profile rounded-circle prep" src="${isNullOrUndefined(val.user.extraProperties.photo) ? rootImagePath + defaultImagePath : rootImagePath + val.user.extraProperties.photo}"/>
                    </div>
                    <div class="d-flex flex-column">
                        <div class="d-flex flex-row">
                           <span class="txtName"><a href="UserConnections/${val.user.userName}">${val.user.name}</a></span>
                             <span class="text-muted px-1 txtUserName"><a href="UserConnections/${val.user.userName}">@${val.user.userName}</a></span>
                                <span class="text-muted px-1 txtTime">&#x26AC;${` `}${getTimeDiffer(`${val.creationTime}`)}</span>
                        </div>
                            <div class="d-flex flex-row">
                                    <span class="text-muted txtIsWatch">&#x26AC; watched</span>
                                    <span class="text-muted txtIsWatch"><i class="fas fa-star"></i>${val.rate}</span>
                            </div>
                    </div>
                        </h5>
                            <div class="row mb-1">
                                <div class="col-11">
                                    <p class="card-text movie-comment">${val.comment}</p>
                                </div>
                            </div>
                            <div class="row mb-1">
                                <div class="col-1">
                                   <img class="card-img-top movieImg" src="https://image.tmdb.org/t/p/original/${val.movie.posterPath}" alt="${val.movie.title}">
                                </div>
                                <div class="col-10">
                                    <div class="row mb-1">
                                     <a href="/Movies/${val.movie.id}"><h6 class="card-subtitle text-muted mb-1">${val.movie.title}</h6></a>
                                    </div>
                                <div class="row mb-1">
                                    <a href="/Movies/Director/${val.movie.id}"><span class="badge bg-success directorPath">ben ksasdasd</span></a>                                    </div>
                                </div>
                            </div>
                    </div>
                </div>
            `)

        })
    }

    isNullOrEmpty = (str) => {
        return str === null || str.match(/^ *$/) !== null;
    }

    isNullOrUndefined= (str) => {
        return str === null || str===undefined;
    }

    loadMore =async (skipCount, maxResultCount) => {

        await $('.btn-load-more').fadeOut().promise();

        movieMatch.posts.post.getFeed({ userId: `${abp.currentUser.id}`, maxCount: maxResultCount, skipCount, skipCount }).done(async (res) => {

            await $('.loader').fadeOut().promise();

            renderResults(res.items);
           

            if (res.items.length < maxResultCount) {
                $('.btn-load-more').prop('disabled', true);
                $('.btn-load-more').html(l('NoMoreData'));
                
            }

            skipCount += res.items.length;

            await $('.btn-load-more').fadeIn().promise();

        });
    }

    $('.btn-load-more').on('click', () => {
        loadMore(skipCount, maxResultCount);
    });

    getTimeDiffer=(creationTime)=>{

        const date = new Date(creationTime);
        const currentTime = new Date();
        const diff = currentTime.getTime() - date.getTime();
        const minute = 60 * 1000
        const hour = 60*minute;
        const day = 24*hour;
        const month = 30*day;

        if (diff < hour) {
            return `${Math.floor(diff/minute)}${l('Minute')}`;
        } else if (diff < day) {
            return `${Math.floor(diff/hour)}${l('Hour')}`;
        } else if (diff < month) {
            return `${Math.floor(diff/day)}${l('Day')}`;
        } 

        return date.toLocaleDateString();
       
    }

    getPosts();
    
});
