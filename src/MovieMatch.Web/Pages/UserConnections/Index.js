istrue = true;
$(function () {
    
    var l = abp.localization.getResource('MovieMatch');
    var dataTable = $('#UserConnectionsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(movieMatch.userConnections.userConnection.getList),

            columnDefs:
                [
                    {
                        title: l('Username'),
                        data: "userName"
                    },
                    {
                        
                        data: "id",
                        render: function (data) {
                            
                            return `<button type="button" id='${data}'  onclick="followUser('${data}')" class="btn btn-success">Follow User</button>`
                            
                            }
                        
                    },
                    
                ]
        })
    );
   // asd = true;
    
    followUser = (id) => {
        (asd) = document.getElementById(id);

        
            
        //followeUser
        movieMatch.userConnections.userConnection.addFollower(id, istrue).done((res) => {
            if (!res) {
                $(asd).css("background-color", "red");
             //   ($(asd).attr("isActive", 'false'));
                $(asd).text("Unfollow User");
                istrue = !istrue;
                abp.notify.success(`Followed user `);

            }
            else {
                $(asd).css("background-color", "green");
            //    ($(asd).attr("isActive", 'true'));
                $(asd).text("follow User");
                istrue = !istrue;
                abp.notify.success(`Unfollowed user `);
            }


        });
                
                    
                      
                
              
            }
        
      
            

           
        

      
    


});