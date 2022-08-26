const defaultPicturePath ="/images/host/my-file-container/default_picture.png"

var l = abp.localization.getResource('MovieMatch');

var dataTable = $('#UserConnectionsTable').DataTable(
    abp.libs.datatables.normalizeConfiguration({
        serverSide: true,
        paging: true,
        order: [[1, "asc"]],
        searching: true,
        scrollX: true,
        ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getList),
        
        columnDefs:
            [
                
                {
                    data: "path",
                    
                    render: function (data) {
                            return `<img class="profile rounded-circle" src="${data}"/>`
                     
                    }
                },
                {
                      title: l('Username'),
                    data: "name",
                    render: function (name) {

                    

                        return `<a  href='/UserConnections/${name}' style="text-transform:capitalize" /* onclick="changeinfo(this)"*/ > ${name} </a>`
                    }

                },

                {


                    data: "isFollow",
                    render: function (data, type, row) {
                        isActive = row.isFollow; var id = row.id;

                        if (isActive === false) {
                            return `<button type="button" id='${(id)}' isActive="false" onclick="followUser(this)" class="btn btn-outline-info">Follow User</button>`

                        }
                        else {
                            return `<button type="button" id='${(id)}' isActive="true" onclick="followUser(this)" class="btn btn-outline-info">UnFollow User</button>`

                        }
                    }

                }


            ]

    })
);

followUser = (button) => {
    var btn = $(button);
    var id = btn.attr("id");

    if ((btn).attr("isActive") == 'false') {
        movieMatch.userConnections.userConnection.follow(btn.attr("id"), btn.attr("isActive")).done(() => {
            (btn).attr("class", "btn btn-outline-info");

            (btn).text("UnFollow User");
            (btn).attr("isActive", 'true');

            abp.notify.success('Followed user');

          
            $('#UserConnectionsTable').DataTable().ajax.reload();

        });
    }
    else {
        movieMatch.userConnections.userConnection.unFollow(id, isActive).done(() => {
            (btn).attr("class", "btn btn-outline-info");
            (btn).attr("isActive", 'false');

            (btn).text("Follow User");

            abp.notify.success(`UnFollowed user`);

            $('#UserConnectionsTable').DataTable().ajax.reload();
        });
    }
}


var swiper = new Swiper(".user-carousel", {
    slidesPerView: 5,
    spaceBetween: 10,
    centeredSlides: true,
    allowTouchMove: true,
    allowClick: true,
    loop: true,
    direction: 'horizontal',
    autoHeight: true,
    centeredSlides: true,
    pagination: {
        el: '.swiper-pagination',
        clickable: true
    },
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});