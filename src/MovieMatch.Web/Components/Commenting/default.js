(function ($) {
    var wasSubmitted = false;  
    var l = abp.localization.getResource('CmsKit');

    abp.widgets.CmsCommenting = function ($widget) {
        var widgetManager = $widget.data('abp-widget-manager');
        var $commentArea = $widget.find('.cms-comment-area');


        function getFilters() {
            return {
                entityType: $commentArea.attr('data-entity-type'),
                entityId: $commentArea.attr('data-entity-id')
            };
        }

        function registerEditLinks($container) {
            
            $container.find('.comment-edit-link').each(function () {
                var $link = $(this);
                $link.on('click', function (e) {
                    e.preventDefault();
                    var commentId = $link.data('id');

                    var $relatedCommentContentArea = $container.find('.cms-comment-content-area[data-id=' + commentId + ']');
                    var $relatedCommentEditFormArea = $container.find('.cms-comment-edit-area[data-id=' + commentId + ']');

                    $relatedCommentContentArea.hide();
                    $relatedCommentEditFormArea.show();
                    $link.removeAttr('href');
                });
            });
            $container.find('.comment-edit-cancel-button').each(function () {
                var $button = $(this);
                $button.on('click', function (e) {
                    e.preventDefault();

                    var commentId = $button.data('id');

                    var $relatedCommentContentArea = $container.find('.cms-comment-content-area[data-id=' + commentId + ']');
                    var $relatedCommentEditFormArea = $container.find('.cms-comment-edit-area[data-id=' + commentId + ']');
                    var $link = $container.find('.comment-edit-link[data-id=' + commentId + ']');

                    $relatedCommentContentArea.show();
                    $relatedCommentEditFormArea.hide();
                    $link.attr('href', '#');
                });
            });
        }

        function registerReplyLinks($container) {
            $container.find('.comment-reply-link').each(function () {
                var $link = $(this);
                $link.on('click', function (e) {
                    e.preventDefault();
                    var replyCommentId = $link.data('reply-id');

                    var $relatedCommentArea = $container.find('.cms-comment-form-area[data-reply-id=' + replyCommentId + ']');
                    var $links = $container.find('.comment-reply-link[data-reply-id=' + replyCommentId + ']');

                    $relatedCommentArea.show();
                    $relatedCommentArea.find('textarea').focus();
                    $links.addClass('disabled');
                });
            });
            $container.find('.reply-cancel-button').each(function () {
                var $button = $(this);
                $button.on('click', function (e) {
                    e.preventDefault();

                    var replyCommentId = $button.data('reply-id');

                    var $relatedCommentArea = $container.find('.cms-comment-form-area[data-reply-id=' + replyCommentId + ']');
                    var $links = $container.find('.comment-reply-link[data-reply-id=' + replyCommentId + ']');

                    $relatedCommentArea.hide();
                    $links.removeClass('disabled');
                });
            });
        }

        function registerDeleteLinks($container) {
            $container.find('.comment-delete-link').each(function () {
                var $link = $(this);
                $link.on('click', '', function (e) {
                    e.preventDefault();
                    var $relatedCommentReplyArea = $(`#${$link.data('id') }.comment-stars`)
                    abp.message.confirm(l("MessageDeletionConfirmationMessage"), function (ok) {
                        if (ok) {
                            volo.cmsKit.public.comments.commentPublic.delete($link.data('id')
                            ).then(function () {
                                var id = $link.data('id');
                                abp.notify.info(l('SuccessfullyDeleted'));
                                //$("#" + id).css("display", "none");
                                $relatedCommentReplyArea.hide()
                            });
                        }
                    });
                });
            });
        }

        function registerUpdateOfNewComment($container) {
            $container.find('.cms-comment-update-form').each(function () {
                var $form = $(this);
                $form.submit(function (e) {
                    e.preventDefault();
                    var formAsObject = $form.serializeFormToObject();
                    volo.cmsKit.public.comments.commentPublic.update(
                        formAsObject.id,
                        {
                            text: formAsObject.commentText,
                            //concurrencyStamp: formAsObject.commentConcurrencyStamp
                        }
                    ).then(function (data) {
                        //$("#" + "cms-comment_Movie_" + data.entityId + "_" + data.id).css("display", "none")
                        var $relatedCommentEditArea = $container.find('.cms-comment-edit-area[data-id=' + data.id + ']');
                        var $relatedCommentContentArea = $container.find('.cms-comment-content-area[data-id=' + data.id + ']');
                        $relatedCommentEditArea.hide();
                        $relatedCommentContentArea.show();
                        debugger;
                        $(`#edit_${data.id}>p`).text(data.text)

                    });
                });
            });
        }

        function registerSubmissionOfNewComment($container) {
            $container.find('.cms-comment-form').each(function () {
                var $form = $(this);
                $form.submit(function (e) {
                    if (!wasSubmitted) {
                        e.preventDefault();
                        var formAsObject = $form.serializeFormToObject();

                        if (formAsObject.repliedCommentId == '') {
                            formAsObject.repliedCommentId = null;
                        }

                        wasSubmitted = true;
                        volo.cmsKit.public.comments.commentPublic.create(
                            $commentArea.attr('data-entity-type'),
                            $commentArea.attr('data-entity-id'),
                            {
                                repliedCommentId: formAsObject.repliedCommentId,
                                text: formAsObject.commentText
                            }
                        ).then(function (data) {
                            //reply or new comment control
                            if ($form[0][3]) {
                                widgetManager.refresh()
                                currentPage--;
                            }
                            else {
                                currentPage = 1;
                                $.ajax({
                                    url: '/Comments/MyViewComponent/',
                                    data: { entityType: "Movie", entityId: data.entityId, currPage: currentPage },
                                    type: 'GET',
                                    success: function (data) {
                                        $("#cms-comment").empty();
                                        $("#cms-comment").prepend(data);
                                    }
                                })
                            }

                        });
                    }
                });
                wasSubmitted = false;
            });
        }

        function focusOnHash($container) {
            if (!location.hash.toLowerCase().startsWith('#cms-comment')) {
                return;
            }

            var $link = $(location.hash + '_link');

            if ($link.length > 0) {
                $link.click();
            }
            else {
                $(location.hash).find('textarea').focus();
            }
        }

        function init() {
            registerReplyLinks($widget);
            registerEditLinks($widget);
            registerDeleteLinks($widget);

            registerUpdateOfNewComment($widget);
            registerSubmissionOfNewComment($widget);

            focusOnHash($widget);
        }

        return {
            init: init,
            getFilters: getFilters
        };

           

    };

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
            var id = $("#cms-comment").attr("data-content");
            currentPage++;
            $.ajax({

                url: '/Comments/MyViewComponent/',
                data: { entityType: "Movie", entityId: id, currPage: currentPage },
                type: 'GET',
                success: function (data) {
                    var pos = data.search('<div class="comment-stars"');
                    if (pos == -1) {
                        currentPage--;
                    }
                    else {
                        $("#cms-comment").append(data.substr(pos));
                        abp.widgets.CmsCommenting($("#nav-home")).init();
                    }
                   

                }
            })

        }
    });
})(jQuery);


       