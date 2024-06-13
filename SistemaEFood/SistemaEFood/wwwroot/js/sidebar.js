

function openSidebar() {
    document.getElementById("sidebarContainer").style.display = "block";

    document.getElementById("sideBar-openButton").classList.add("active");
    document.getElementById("sidebar").classList.add("open");
    var sidebarOverlay = document.getElementById("sidebar-overlay");
    sidebarOverlay.style.display = 'block';
    setTimeout(() => {
        sidebarOverlay.classList.add('visible');
    }, 10);
}
function checkSideBar() {
    var sidebar = document.getElementById("sidebar");
    if (sidebar.classList.contains("open")) {
      
        closeSidebar();
    }
    else {
        openSidebar();
    }
}
function closeSidebar() {
    document.getElementById("sideBar-openButton").classList.remove("active");
    document.getElementById("sidebar").classList.remove("open");
    var sidebarOverlay = document.getElementById("sidebar-overlay");
    sidebarOverlay.classList.remove('visible');
    setTimeout(() => {
        sidebarOverlay.style.display = 'none';
    }, 800); 
  //  document.getElementById("sidebarContainer").style.display = "none";
    

}
function openMenuItem(id) {
    document.getElementById(id).style.display = "flex";
}

function closeMenuItem(id) {
    document.getElementById(id).style.display = "none";
}
