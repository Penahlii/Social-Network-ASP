const searchFriendRequest = document.getElementById("searchFriendRequests");
searchFriendRequest.addEventListener('input', (e) => {
    const searchValue = e.target.value.trim(); 
    if (searchValue !== "") {
        updateFriendRequests(searchValue);
        updatePeopleYouKnow(searchValue);
    } else {
       
        updateFriendRequests(""); 
         updatePeopleYouKnow("");
    }
});


async function getSentFriendRequests() {
    let sentFriendRequests = null;
    await $.ajax({
       
        url: "https://localhost:7114/api/FriendRequest/GetAllSentFriendRequestsOfCurrentUser",
            contentType: "application/json",
            method: "GET",
        success: function (response) {
            sentFriendRequests = response.sentFriendRequests;
        },
        error: function () {
            console.error("error while getting sent friend requests of current user");
        }
    });
    return sentFriendRequests;
}

async function getFriendRequests(key) {
    let friendRequests = null;
    await $.ajax({
        url: "https://localhost:7114/api/FriendRequest/GetAllFriendRequestsOfCurrentUser",
        contentType: "application/json",
        method: "GET",
        data:{
            key: key
        },
        success: function (response) {
            friendRequests = response.friendRequests;
        },
        error: function () {
            console.error("error while getting friend requests of current user");
        }
    });
    return friendRequests;
}


async function getAllFriendRequests() {
    let allFriendRequests = null;
    await $.ajax({
        url: "https://localhost:7114/api/FriendRequest/GetAllFriendRequests",
        contentType: "application/json",
        method: "GET",
        success: function (response) {
            allFriendRequests = response.allFriendRequests;
        },
        error: function () {
            console.error("error while getting all friend requests from db");
        }
    });
    return allFriendRequests;
}

async function getFriends(key) {
    let friends = null;
    await $.ajax({
        url: "https://localhost:7114/api/Friend/GetAllFriendsOfCurrentUser",
        contentType: "application/json",
        method: "GET",
        data: {
           key : key
        },
        success: function (response) {
            friends = response.allFriends;
        },
        error: function () {
            console.error("error while getting friends of current user");
        }
    });
    return friends;
}

async function getCurrentUser() {
    let user = null;
    await $.ajax({
        url: "https://localhost:7114/Account/GetCurrentUser",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            user = response.user;
        },
        error: function () {
            console.error("error while getting current user .");
        }
    });
    return user;
}

async function getOtherPeople(key) {
    let users = null;
    await $.ajax({
        url: "https://localhost:7114/api/Friend/GetOtherPeople",
        method: "GET",
        contentType: "application/json",
        data: {
            key : key,
        },
        success: function (response) {
            users = response.otherPeople;
        },
        error: function () {
            console.error("error while getting current user .");
        }
    });
    return users;
}

async function updateFriendRequests(key) {
    const friendRequests = await getFriendRequests(key);
    const currentUser = await getCurrentUser();
    const allFriendRequests = await getFriendRequests("");
    console.log("Friend Requests", friendRequests);
    let content = "";

    for (let i = 0; i < friendRequests.length; i++) {
        const request = friendRequests[i];
        const otherUser = request.sender.id === currentUser.id ? request.receiver : request.sender;
        let buttons = "";

        if (request.sender.id === currentUser.id) {
            buttons = `
                <div class="add-friend-btn">
                    <button onclick="cancelFriendRequest('${otherUser.id}')">Cancel Request</button>
                </div>
                <div class="send-message-btn">
                    <button onclick="sendMessageToUser('${otherUser.id}')">Send Message</button>
                </div>`;
        }
        else if (request.receiver.id === currentUser.id) {
            buttons = `
                <div class="add-friend-btn">
                    <button onclick="acceptFriendRequest('${otherUser.id}')">Accept</button>
                    <button onclick="rejectFriendRequest('${otherUser.id}')">Reject</button>
                </div>
                <div class="send-message-btn">
                    <button onclick="sendMessageToUser('${otherUser.id}')">Send Message</button>
                </div>`;
        }

        content += `
        <div class="col-lg-3 col-sm-6">
            <div class="single-friends-card">
                <div class="friends-image">
                    <a href="#">
                        <img src="/images/${otherUser.backgroundImageUrl}.jpg" alt="image">
                    </a>
                    <div class="icon">
                        <a href="#"><i class="flaticon-user"></i></a>
                    </div>
                </div>
                <div class="friends-content">
                    <div class="friends-info d-flex justify-content-between align-items-center">
                        <a href="#">
                            <img src="/images/${otherUser.imageUrl}" alt="image">
                        </a>
                        <div class="text ms-3">
                            <h3><a href="#">${otherUser.userName}</a></h3>
                        </div>
                    </div>
                    <ul class="statistics">
                        <li>
                            <a href="#">
                                <span class="item-number">${otherUser.posts.flatMap(p => p.postLikes).length}</span>
                                <span class="item-text">Likes</span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="item-number">${allFriendRequests.filter(fr => fr.senderId === otherUser.id).length}</span>
                                <span class="item-text">Following</span>
                            </a>
                        </li>
                        <li>
                            <a href="#">
                                <span class="item-number">${allFriendRequests.filter(fr => fr.receiverId === otherUser.id).length}</span>
                                <span class="item-text">Followers</span>
                            </a>
                        </li>
                    </ul>
                    <div class="button-group d-flex justify-content-between align-items-center">
                        ${buttons}
                    </div>
                </div>
            </div>
        </div>`;
    }
    document.getElementById("friendRequests").innerHTML = content;
}

async function sendFriendRequest(receiverId) {
    $.ajax({
        url: "https://localhost:7114/api/FriendRequest/SendFriendRequest",
        contentType: "application/json",
        method: "GET",
        data: {
            receiverId: receiverId
        },
        success: async function () {
            console.log("friend request has been sent .");
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            const currentUser = await getCurrentUser();
            sendNotification(currentUser.id, receiverId, "send a friend request");
        },
        error: function () {
            console.error("error while sending friend request");
        }
    });
}

function cancelFriendRequest(receiverId) {
    $.ajax({
        url: "https://localhost:7114/api/FriendRequest/CancelFriendRequest",
        contentType: "application/json",
        method: "GET",
        data: {
            receiverId: receiverId
        },
        success:async function () {
            console.log("friend request canceled .");
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            const currentUser = await getCurrentUser();
            sendNotification(currentUser.id, receiverId, "canceled a friend request");
        },
        error: function () {
            console.error("error while canceling friend request");
        }
    });
}

function acceptFriendRequest(senderId) {
    $.ajax({
        url: "https://localhost:7114/api/FriendRequest/AcceptFriendRequest",
        contentType: "application/json",
        method: "GET",
        data: {
            senderId: senderId
        },
        success: async function () {
            console.log("friend accepted .");
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            const currentUser = await getCurrentUser();
            sendNotification(currentUser.id, senderId, "accepted a friend request");
        },
        error: function () {
            console.error("error while accepting friend request");
        }
    });
}

async function rejectFriendRequest(senderId) {
    await $.ajax({
        url: "https://localhost:7114/api/FriendRequest/RejectFriendRequest",
        contentType: "application/json",
        method: "GET",
        data: {
            senderId: senderId
        },
        success: async function () {
            const currentUser = await getCurrentUser();
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            sendNotification(currentUser.id, senderId ,"rejected a friend request");
        },
        error: function () {
            console.error("error while rejecting friend request");
        }
    });
}

function sendMessageToUser(userId) {
    window.location.href = "/Home/Messages";
    updateSpecialChat(userId);
}

async function updatePeopleYouKnow(key) {
    const users = await getOtherPeople(key);
    console.log("People you know ",users);
    const allFriendRequests = await getFriendRequests("");
    let content = "";
    for (let i = 0; i < users.length; i++) {
        content += `
       <div class="col-lg-3 col-sm-6">
    <div class="single-friends-card">
        <div class="friends-image">
            <a href="#">
                <img src="/images/${users[i].backgroundImageUrl}" alt="image">
            </a>
            <div class="icon">
                <a href="#"><i class="flaticon-user"></i></a>
            </div>
        </div>
        <div class="friends-content">
            <div class="friends-info d-flex justify-content-between align-items-center">
                <a href="#">
                    <img src="/images/${users[i].imageUrl}" alt="image">
                </a>
                <div class="text ms-3">
                    <h3><a href="#">${users[i].userName}</a></h3>
                </div>
            </div>
            <ul class="statistics">
                <li>
                    <a href="#">
                        <span class="item-number">${users[i].posts.flatMap(p => p.postLikes).length}</span>
                        <span class="item-text">Likes</span>
                    </a>
                </li>
                <li>
                    <a href="#">
                        <span class="item-number">${allFriendRequests.filter(fr => fr.senderId === users[i].id).length}</span>
                        <span class="item-text">Following</span>
                    </a>
                </li>
                <li>
                    <a href="#">
                        <span class="item-number">${allFriendRequests.filter(fr => fr.receiverId === users[i].id).length}</span>
                        <span class="item-text">Followers</span>
                    </a>
                </li>
            </ul>
            <div class="button-group d-flex justify-content-between align-items-center">
                <div class="add-friend-btn">
                    <button onclick="sendFriendRequest('${users[i].id}')">Send Friend Request</button>
                </div>
                <div class="send-message-btn">
                    <button onclick="sendMessageToUser('${users[i].id}')">Send Message</button>
                </div>
            </div>
        </div>
    </div>
</div>`; 
    }
    $("#peopleYouKnow").html(content);
}

async function updateFriends(key) {
    console.log("friends updated !");
    const currentUser = await getCurrentUser();
    const friends = await getFriends(key);
    const allFriendRequests = await getAllFriendRequests();
    let content = "";
    for (let i = 0; i < friends.length; i++) {
        const friend = friends[i].ownId === currentUser.id ? friends[i].yourFriend : friends[i].own;
        content += `
<div class="col-lg-3 col-sm-6">
    <div class="single-friends-card">
        <div class="friends-image">
            <a href="#">
                <img src="/images/${friend.backgroundImageUrl}" alt="image">
            </a>
            <div class="icon">
                <a href="#"><i class="flaticon-user"></i></a>
            </div>
        </div>
        <div class="friends-content">
            <div class="friends-info d-flex justify-content-between align-items-center">
                <a href="#">
                    <img src="/images/${friend.imageUrl}" alt="image">
                </a>
                <div class="text ms-3">
                    <h3><a href="#">${friend.userName}</a></h3>
                </div>
            </div>
            <ul class="statistics">
                <li>
                    <a href="#">
                        <span class="item-number">${friend.posts.flatMap(p => p.postLikes).length}</span>
                        <span class="item-text">Likes</span>
                    </a>
                </li>
                <li>
                    <a href="#">
                        <span class="item-number">${allFriendRequests.filter(fr => fr.senderId === friend.id).length}</span>
                        <span class="item-text">Following</span>
                    </a>
                </li>
                <li>
                    <a href="#">
                        <span class="item-number">${allFriendRequests.filter(fr => fr.receiverId === friend.id).length}</span>
                        <span class="item-text">Followers</span>
                    </a>
                </li>
            </ul>
            <div class="button-group d-flex justify-content-between align-items-center">
                <div class="send-message-btn">
                    <button onclick="sendMessageToUser('${friend.id}')">Send Message</button>
                </div>
            </div>
        </div>
    </div>
</div>
        `;
    }
    $("#allFriends").html(content);
}

async function removeFriend(friendId) {
    await $.ajax({
        url: "https://localhost:7114/api/Friend/RemoveFriend",
        contentType: "application/json",
        method: "GET",
        data: {
            friendId: friendId
        },
        success: async function () {
            console.log("friend removed .");
            await connection.invoke("UpdateFriendRequestsAndFriendsForUsers");
            await connection.invoke("UpdateContactsForAllUsers");
            sendNotification(currentUser.id, receiverId, "removed you from friends");
        }
    });
}

async function updateAllAboutFriends() {
    console.log("all about friends updated .");
    await updateFriends("");
    await updatePeopleYouKnow("");
    await updateFriendRequests("");
    $("#searchFriendRequests").val("");
}

updateAllAboutFriends();